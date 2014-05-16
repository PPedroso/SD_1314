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
    static class Broker
    {
        static readonly int DEFAULT_NUMBER_OF_WORKERS = 4;

        static void initWorkers(){
            for (int i = 0; i < DEFAULT_NUMBER_OF_WORKERS;++i )
                addWorker();
        }

        static void addWorker() {
            DictionaryWrapper myDict = DictionaryWrapper.getInstance();
            int NUMBER_OF_MAX_SLOTS_FOR_WORKER = 4;
            myDict.addWorker(NUMBER_OF_MAX_SLOTS_FOR_WORKER);
        }

        static void removeWorker(int port) {
            DictionaryWrapper myDict = DictionaryWrapper.getInstance();
            myDict.removeWorker(port);

        }
        
        static void listWorkers(){
            DictionaryWrapper myDict = DictionaryWrapper.getInstance();
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
                initWorkers();
                Console.WriteLine("Broker is working in automatic mode, press any key to shut down");
                Console.ReadLine();
            }
            else {
                while (option != "e") {
                    Console.WriteLine("\n Add Worker: a" + 
                                      "\n Remove Worker: r (not working yet)" +
                                      "\n List current Workers: l" +
                                      "\n Exit: e");
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
        private DictionaryWrapper myDict = DictionaryWrapper.getInstance();

        public void finishJob(int port,long id) {
            Console.WriteLine("Here be monsters");
            myDict.setJobStatusFinished(id);
            myDict.removeJobFromWorker(port, id);
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

            long id = myDict.getCurrentJobId();
            j.setJobId(id);
            
            Task t = Task.Run( () => myDict.add(j));
            
            Console.WriteLine("New Job Added (" + id + ")");
            Console.WriteLine(j.getJobDescription());

            return id;
        }
    }
}