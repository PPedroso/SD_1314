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
            addWorker();
        }

        static void addWorker() {
            DictionaryWrapper myDict = DictionaryWrapper.getInstance();
            int NUMBER_OF_MAX_SLOTS_FOR_WORKER = 4;

            myDict.addWorker(NUMBER_OF_MAX_SLOTS_FOR_WORKER);
        }
        
        static void Main()
        {
            string brokerClientConfigFile = "Broker.exe.config";
            RemotingConfiguration.Configure(brokerClientConfigFile, false);
            initWorkers();
            Console.WriteLine("Broker is working, press any key to shut down");
            Console.ReadLine();
        }
    }

    public class MyBrokerCallbackObject : MarshalByRefObject, IBrokerCallback {
        private DictionaryWrapper myDict = DictionaryWrapper.getInstance();

        public void finishJob(long id) {
            myDict.setJobStatusFinished(id);
            
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