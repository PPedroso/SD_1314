using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using JobImplementation;
using WorkerSAO;
using System.IO;
using System.Diagnostics;

namespace Broker
{
    public class DictionaryWrapper
    {
        private static DictionaryWrapper dw = new DictionaryWrapper();
        public static DictionaryWrapper getInstance() { return dw; }

        //readonly Dictionary<long, Job> dict = new Dictionary<long, Job>();
        //readonly LinkedList<JobWrapper> dict = new LinkedList<JobWrapper>();

        Dictionary<long, JobWrapper> jobDict = new Dictionary<long, JobWrapper>();
        Dictionary<int, WorkerWrapper> workerDict = new Dictionary<int, WorkerWrapper>();


        long jobId = 0;
        readonly Object genericLockObject = new Object();

        private DictionaryWrapper() { } //Certificação que mais ninguem cria uma instancia

        public long getCurrentJobId()
        {
            long nId = Interlocked.Increment(ref jobId);
            return nId;
        }

        public void addWorker(int max_slots ) { 
            
            //Max slots ainda não está a ser usado
            //Alterar para que a porta seja gerada aqui
            int port = baseWorkerPort + ++numberOfWorkers;
            IWorkerSAO worker = createWorker(port);
            workerDict.Add(port,new WorkerWrapper(worker));
        }

        static int baseWorkerPort = 2000;
        static int numberOfWorkers = 0;

        static IWorkerSAO createWorker(int port)
        {
            //Criação do processo worker
            string configFilePath = createConfigFile(port);
            ProcessStartInfo processInfo = new ProcessStartInfo(
                "C:\\Users\\Pedro\\Escola\\SD\\Series\\PrimeiraSerie\\Worker\\bin\\Debug\\Worker.exe");
            processInfo.Arguments = configFilePath;
            Process newWorker = Process.Start(processInfo);
            Console.WriteLine("Starting worker " + numberOfWorkers);

            //Criação do proxy para o worker
            return (IWorkerSAO)Activator.GetObject(typeof(IWorkerSAO), "tcp://localhost:" + (baseWorkerPort + numberOfWorkers) + "/JobWorker");
        }


        private static string createConfigFile(int port)
        {
            //Para alterar para a configuração programática
            
            StreamWriter file = new StreamWriter("WorkerConfig" + port + ".exe.config");
            file.WriteLine(@"<?xml version=""1.0""?>");
            file.WriteLine("<configuration>");
            file.WriteLine("<system.runtime.remoting>");
            file.WriteLine("<application>");
            file.WriteLine("<channels>");
            file.WriteLine(@"<channel ref=""tcp"" port=""" + port + @""">");
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
            return Path.GetFullPath("WorkerConfig" + numberOfWorkers + ".exe.config");
        }


        public void setJobStatusFinished(long jobId) {
            jobDict[jobId].setJobStatusFinished();
        }

        public string requestJobStatus(long id)
        {
            JobWrapper jw;
            jobDict.TryGetValue(id, out jw);
            if (jw == null) return "Job not found";
            return jw.getJobStatus();
        }

        public long add(Job j)
        {
            JobWrapper jw = new JobWrapper(j);
            lock (genericLockObject)
            {
                jobDict.Add(j.getJobId(), jw);
            }
            workerDict.ElementAt(0).Value.addJob(j);
            return jobId;
        }

        //public JobWrapper removeFirst()
        //{
        //    lock (genericLockObject)
        //    {
        //        LinkedListNode<JobWrapper> jw = dict.First;
        //        dict.RemoveFirst();
        //        return jw.Value;
        //    }
        //}

        //public void remove(long id)
        //{
        //    JobWrapper jw = getJobWrapper(id);

        //    lock (genericLockObject)
        //    {
        //        dict.Remove(jw);
        //    }
        //}

        //public Job getFirst()
        //{
        //    return getFirstWrapper().getJob();
        //}

        //public JobWrapper getFirstWrapper()
        //{
        //    LinkedListNode<JobWrapper> jw = dict.First;
        //    return jw.Value;
        //}

        //public JobWrapper getJobWrapper(long id)
        //{
        //    foreach (JobWrapper jw in dict)
        //    {
        //        if (jw.getJob().getJobId() == id)
        //            return jw;
        //    }
        //    return null;
        //}

        //public Job getJob(long id)
        //{
        //    JobWrapper jw = getJobWrapper(id);
        //    return jw.getJob();

        //}
    }
}
