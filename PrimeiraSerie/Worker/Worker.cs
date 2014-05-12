using System;
using WorkerSAO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using JobImplementation;
using System.Threading;

namespace Worker
{
    class Worker
    {

        private void processJob(Job j)
        {
            //Do work

            //Call client endpoint
        }
        
        static void Main(string[] args)
        {
            string configFile = args[0];
            RemotingConfiguration.Configure(configFile, false);

            Console.WriteLine("I am a worker and I am working. Press any key for me to stop working...");
            Console.ReadLine();

        }
    }

    public class MyWorkerObject : MarshalByRefObject, IWorkerSAO
    {
        private volatile int currentJobs = 0;
        
        public int getCurrentNumberOfJobs() {
            return currentJobs;
        }

        public void submitJob(Job j)
        {
            Console.WriteLine("Job submited: " +j.getJobDescription());
            Interlocked.Increment(ref currentJobs);
            j.getEndJob().finish(j.getJobId());
        }
    }
}
