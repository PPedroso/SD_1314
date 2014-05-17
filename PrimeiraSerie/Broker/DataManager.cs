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
using System.Net.Sockets;

namespace Broker
{
    public class DataManager
    {
        private static DataManager dw = new DataManager();
        public static DataManager getInstance() { return dw; }

        Dictionary<long, JobWrapper> jobDict = new Dictionary<long, JobWrapper>();
        Dictionary<int, WorkerWrapper> workerDict = new Dictionary<int, WorkerWrapper>();

        long jobId = 0;
        readonly Object genericLockObject = new Object();

        private DataManager() { } //Certificação que mais ninguem cria uma instancia

        //
        //------------------------------- GETTERS -------------------------------------------------------
        //

        public Dictionary<int, WorkerWrapper> getWorkers() {
            return workerDict;
        }

        private WorkerWrapper getWorkerWrapper() {
            //Soluçao temporária
            lock (genericLockObject) {
                //int id = (int)(jobId % workerNr);
                //WorkerWrapper wrapper = workerDict.ElementAt(id).Value;
                WorkerWrapper wrapper = workerDict.OrderBy((x) => x.Value.getCurrentJobs()).First().Value;
                if (wrapper.getCurrentJobs() == Broker.NUMBER_OF_MAX_SLOTS_FOR_WORKER) {
                    wrapper = addWorker(Broker.NUMBER_OF_MAX_SLOTS_FOR_WORKER);
                }
                return wrapper;
            }
        }


        public long getCurrentJobId() 
        {
            long nId = Interlocked.Increment(ref jobId);
            return nId;
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

        public void add(Job j) {
            //JobWrapper jw = new JobWrapper(j);
            //bool succeded = false;
            //while (!succeded) {
            //    WorkerWrapper wrapper = getWorkerWrapper();
            //    lock (wrapper) {
            //        if (wrapper.getIsFunctioning()) {
            //            try {
            //                wrapper.addJob(j);
            //            } catch (SocketException) {
            //                wrapper.setIsFunctioning(false);
            //                removeWorker(wrapper.getPort());
            //            }
            //        }
            //    }
            //}

            

            JobWrapper jw = new JobWrapper(j);
            lock (genericLockObject) {
                if(!jobDict.ContainsKey(j.getJobId()))
                    jobDict.Add(j.getJobId(), jw);
                WorkerWrapper wrapper = null;
                bool succeded = false;
                while (!succeded) {
                    try {
                        wrapper = getWorkerWrapper();
                        wrapper.addJob(j);
                        
                        succeded = true;
                    } catch (SocketException) {
                        removeWorker(wrapper.getPort());
                    }
                }
            }            
        }

        public void removeJobFromWorker(int port, long jobId) {
            lock (genericLockObject) {
                workerDict[port].removeJob(jobId);
            }   
        }

        //
        //---------------------------------------------- ADD/REMOVE WORKERS ---------------------------------------------------
        //

        int baseWorkerPort = 2000;
        int workerNr = 0;

        public WorkerWrapper addWorker(int max_slots)
        {

            //Max slots ainda não está a ser usado

            int port = baseWorkerPort + ++workerNr;
            IWorkerSAO worker = createWorker(port);
            WorkerWrapper wrapper = new WorkerWrapper(worker, port);
            lock (genericLockObject) {
                workerDict.Add(port, wrapper);
            }
            return wrapper;
        }

        public void removeWorker(int port)
        {
            IEnumerable<Job> jobList = null;
            lock (genericLockObject) {
                //No caso de um worker ser removido e ainda ter trabalhos por acabar
                jobList = workerDict[port].getJobList();

                try {
                    workerDict[port].getWorkerSAO().closeWorker();
                }
                catch(Exception e){
                    Console.WriteLine(e.Message);
                }
                
                workerDict.Remove(port);
                
                //No caso de ser o ultimo worker a ser removido, adicionar um novo worker
                if (workerDict.Count == 0) {
                    addWorker(Broker.NUMBER_OF_MAX_SLOTS_FOR_WORKER);
                }
            }
            foreach (Job j in jobList) {
                Console.WriteLine("Job re-scheduled: " + j.getJobId());
                Task.Factory.StartNew(() => add(j));
            }
            
        }

        private IWorkerSAO createWorker(int port)
        {
            //Criação do processo worker
            string configFilePath = createConfigFile(port);
            ProcessStartInfo processInfo = new ProcessStartInfo("..\\..\\..\\Worker\\bin\\Debug\\Worker.exe");
            processInfo.Arguments = configFilePath + " " + port;
            Process newWorker = Process.Start(processInfo);
            Console.WriteLine("Starting worker " + workerNr);

            //Criação do proxy para o worker
            return (IWorkerSAO)Activator.GetObject(typeof(IWorkerSAO), "tcp://localhost:" + port + "/JobWorker");
        }

        private string createConfigFile(int port)
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
            return Path.GetFullPath("WorkerConfig" + port + ".exe.config");
        }

    }
}
