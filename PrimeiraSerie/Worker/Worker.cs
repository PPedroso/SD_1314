using System;
using WorkerSAO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using JobImplementation;

namespace Worker
{
    class Worker
    {
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
        public bool ResquestJobStatus()
        {
            throw new NotImplementedException();
        }

        public void submitJob(Job j)
        {
            throw new NotImplementedException();
        }
    }
}
