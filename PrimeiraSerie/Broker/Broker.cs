using System;
using BrokerSAO;
using WorkerSAO;
using BrokerCallback;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.Threading.Tasks;
using JobImplementation;
using System.IO;
using System.Diagnostics;


namespace Broker
{   
    public static class Broker
    {
        public static bool AUTOMATIC_MODE = false;
        static readonly int DEFAULT_NUMBER_OF_WORKERS = 1;
        public static readonly int NUMBER_OF_MAX_SLOTS_FOR_WORKER = 2;

        static void initWorkers(){
            for (int i = 0; i < DEFAULT_NUMBER_OF_WORKERS;++i )
                addWorker();
        }

        static void addWorker() {
            DataManager myDict = DataManager.getInstance();            
            myDict.addWorker(NUMBER_OF_MAX_SLOTS_FOR_WORKER);
        }

        static void removeWorker(int port) {
            DataManager myDict = DataManager.getInstance();
            myDict.removeWorker(port,true);

        }
        
        static void listWorkers(){
            DataManager myDict = DataManager.getInstance();
            Dictionary<int, WorkerWrapper> workers = myDict.getWorkers();

            Dictionary<int, WorkerWrapper>.KeyCollection.Enumerator keyList = workers.Keys.GetEnumerator();
            
            int cont=0;
            while (keyList.MoveNext())
            {
                Console.WriteLine("\n Worker:" + ++cont +
                                 "\n port:" + keyList.Current);
            }
            
        }
        
        static void Main()
        {
            string brokerClientConfigFile = "Broker.exe.config";
            RemotingConfiguration.Configure(brokerClientConfigFile, false);
            Console.WriteLine("Select mode (auto/manual)");
            string option = Console.ReadLine();
            if (option == "auto")
            {
                AUTOMATIC_MODE = true;
                initWorkers();
                Console.WriteLine("Broker is working in automatic mode, press any key to shut down");
                Console.ReadLine();
            }
            else if(option== "manual" ) {
                while (option != "e") {
                    Console.WriteLine("\n Add Worker: a" + 
                                      "\n Remove Worker: r (not working yet)" +
                                      "\n List current Workers: l" +
                                      "\n Exit: e" +
                                      "\n");
                    option = Console.ReadLine();

                    switch (option) { 
                        case "a":
                            addWorker();
                            break;
                        case "r":
                            Console.WriteLine("Insert worker port");
                            removeWorker(Int32.Parse(Console.ReadLine()));
                            break;
                        case "l":
                            listWorkers();
                            break;
                    }
                }
            }
        }
    }

    public class MyBrokerCallbackObject : MarshalByRefObject, IBrokerCallback {
        private DataManager myDict = DataManager.getInstance();

        public void finishJob(int port,long id) {
            myDict.setJobStatusFinished(id);
            myDict.removeJobFromWorker(port, id);
        }
    }
    
    public class MyBrokerObject : MarshalByRefObject, IBrokerSAO
    {
        DataManager dataManager = DataManager.getInstance();
        
        public string RequestJobStatus(long jobId)
        {
            return dataManager.requestJobStatus(jobId);
        }

        public long SubmitJob(Job j)
        {

            long id = dataManager.getCurrentJobId();
            j.setJobId(id);
            
            Task t = Task.Run( () => dataManager.add(j));
            
            Console.WriteLine("New Job Added (" + id + ")");
            Console.WriteLine(j.getJobDescription());

            return id;
        }
    }
}