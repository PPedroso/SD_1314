using System;
using BrokerSAO;
using WorkerSAO;
using BrokerCallback;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using JobImplementation;
using System.IO;
using System.Diagnostics;


namespace Broker
{
    public class DictionaryWrapper {
        private static DictionaryWrapper dw = new DictionaryWrapper();
        public static DictionaryWrapper getInstance(){ return dw; }
        
        //readonly Dictionary<long, Job> dict = new Dictionary<long, Job>();
        readonly LinkedList<JobWrapper> dict = new LinkedList<JobWrapper>();
        long jobId = 0;
        readonly Object genericLockObject = new Object();

        private DictionaryWrapper() { } //Certificação que mais ninguem cria uma instancia


        
        public int getCount()
        {
            return dict.Count;
        }

        private long getCurrentJobId()
        {
            long nId = Interlocked.Increment(ref jobId);
            return nId;
        }

        public string requestJobStatus(long id) {
            JobWrapper jw = getJobWrapper(id);
            return jw.getJobStatus();
        }

        public long add(Job j) {
            lock (genericLockObject) {
                long jobId =getCurrentJobId();
                j.setJobId(jobId);

                JobWrapper jw = new JobWrapper(j);
                dict.AddLast(jw);
                return jobId;
            }
        }

        public JobWrapper removeFirst() {
            lock (genericLockObject) {
                LinkedListNode<JobWrapper> jw = dict.First;
                dict.RemoveFirst();
                return jw.Value;
            }
        }

        public void remove(long id) {
            JobWrapper jw = getJobWrapper(id);
            
            lock (genericLockObject) {
                dict.Remove(jw);
            }
        }

        public Job getFirst() {
            return getFirstWrapper().getJob();
        }
        
        public JobWrapper getFirstWrapper() {
            lock (genericLockObject) {
                LinkedListNode<JobWrapper> jw = dict.First;
                return jw.Value;
            }
        }

        public JobWrapper getJobWrapper(long id) {
            foreach (JobWrapper jw in dict)
            {
                if (jw.getJob().getJobId() == id)
                    return jw;
            }
            return null;
        }
        
        public Job getJob(long id){
            JobWrapper jw = getJobWrapper(id);
            return jw.getJob();

        }
    }

    public class JobWrapper {
        private Job j;
        private status jobStatus;

        enum status { QUEUED, RUNNING, FINISHED};

        public JobWrapper(Job j) {
            this.j = j;
            jobStatus = status.QUEUED;
        }

        public Job getJob() {
            return j;
        }

        public void setJobStatusRunning() { jobStatus = status.RUNNING; }

        public void setJobStatusFinished() { jobStatus = status.FINISHED; }

        public string getJobStatus() { return jobStatus.ToString(); }

    }

    
    static class Broker
    {
        
        static int baseWorkerPort = 2000;
        static int numberOfWorkers=0;
        static readonly int DEFAULT_NUMBER_OF_WORKERS = 4;

        public static LinkedList<KeyValuePair<int, IWorkerSAO>> workers = new LinkedList<KeyValuePair<int, IWorkerSAO>>();

        static void initWorkers(){
            addWorker();
        }

        static void initMonitorThread(){
            MonitorThread mt = new MonitorThread();
            Thread t = new Thread(new ThreadStart(mt.doWork));
            t.Start();
        }

        static void addWorker() {
            int NUMBER_OF_MAX_SLOTS_FOR_WORKER = 4;
            KeyValuePair<int, IWorkerSAO> kp = new KeyValuePair<int, IWorkerSAO>(NUMBER_OF_MAX_SLOTS_FOR_WORKER, createWorker());
            workers.AddLast(kp);
        }

        static IWorkerSAO createWorker() {
            //Criação do processo worker
            string configFilePath = createConfigFile();
            ProcessStartInfo processInfo = new ProcessStartInfo(
                "C:\\Users\\Pedro\\Escola\\SD\\Series\\PrimeiraSerie\\Worker\\bin\\Debug\\Worker.exe");
            processInfo.Arguments = configFilePath;
            Process newWorker = Process.Start(processInfo);
            Console.WriteLine("Starting worker "+ numberOfWorkers);

            //Criação do proxy para o worker
            return (IWorkerSAO)Activator.GetObject(typeof(IWorkerSAO), "tcp://localhost:" + (baseWorkerPort + numberOfWorkers) + "/JobWorker");
        }

        //Para fins de debug
        class clientEndJob : MarshalByRefObject, IEndJob
        {
            public void finish(long jobId)
            {
                Console.WriteLine("Mandou fechar o job com id " + jobId);
            }
        }

        
        private static string createConfigFile(){
            StreamWriter file = new StreamWriter("WorkerConfig" + ++numberOfWorkers + ".exe.config");
            file.WriteLine(@"<?xml version=""1.0""?>");
            file.WriteLine("<configuration>");
            file.WriteLine("<system.runtime.remoting>");
            file.WriteLine("<application>");
            file.WriteLine("<channels>");
            file.WriteLine(@"<channel ref=""tcp"" port=""" + (baseWorkerPort+numberOfWorkers) + @""">");
            file.WriteLine("<clientProviders>");
            file.WriteLine(@"<formatter ref=""binary""/>");
            file.WriteLine("</clientProviders>");
            file.WriteLine("<serverProviders>");
            file.WriteLine(@"<formatter ref=""binary"" typeFilterLevel=""Full""/>");
            file.WriteLine("</serverProviders>");
            file.WriteLine("</channel>");
            file.WriteLine("</channels>");
            file.WriteLine("<service>");
            file.WriteLine(@"<wellknown type=""Worker.MyWorkerObject, Worker"" objectUri=""JobWorker"" mode=""Singleton""/>");
            file.WriteLine("</service>");
            file.WriteLine("</application>");
            file.WriteLine("</system.runtime.remoting>");
            file.WriteLine("</configuration>");
            file.Close();
            return Path.GetFullPath("WorkerConfig" +numberOfWorkers + ".exe.config");
        }
        
        static void Main()
        {
            string brokerClientConfigFile = "Broker.exe.config";
            RemotingConfiguration.Configure(brokerClientConfigFile, false);
            initWorkers();
            initMonitorThread();
            Console.WriteLine("Broker is working, press any key to shut down");
            Console.ReadLine();
        }
    }

    public class MyBrokerCallbackObject : MarshalByRefObject, IBrokerCallback {
        private DictionaryWrapper myDict = DictionaryWrapper.getInstance();

        public void finishJob(long id) {
            myDict.getJobWrapper(id).setJobStatusFinished();
            myDict.remove(id);
            Console.WriteLine("Job:" + id + " has finished");
        }
    }
    
    public class MyBrokerObject : MarshalByRefObject, IBrokerSAO
    {
        DictionaryWrapper myDict = DictionaryWrapper.getInstance();
        
        public string RequestJobStatus(long jobId)
        {
            return myDict.requestJobStatus(jobId);
        }

        public long SubmitJob(Job j)
        {

            long id = myDict.add(j);

            Console.WriteLine("New Job Added (" + id + ")");
            Console.WriteLine(j.getJobDescription());

            return id;
        }
    }

    public class MonitorThread
    {
        private DictionaryWrapper myDict = DictionaryWrapper.getInstance();
        readonly Object genericLockWaitObject = new Object();
        
        public void doWork()
        {
            lock (genericLockWaitObject) {
                do
                {
                    if (myDict.getCount() > 0) { 
                        JobWrapper jw = myDict.getFirstWrapper();
                        jw.setJobStatusRunning();
                        Broker.workers.First.Value.Value.submitJob(new KeyValuePair<Job,IBrokerCallback>(jw.getJob(),new MyBrokerCallbackObject()));
                    }
                } while (true);
            }
        }

    }
}