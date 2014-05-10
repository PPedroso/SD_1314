using BrokerSAO;
using JobImplementation;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Threading;

namespace Client
{
    class Client
    {
        class JobManager
        {
            private bool waitingEnd = false;
            private Object lockObj = new object();
            private Dictionary<long, Job> dict = new Dictionary<long,Job>();
            //in case a job ended before it was added on the dictionary
            private LinkedList<long> listEndedBeforeAddingJob = new LinkedList<long>();

            public void addJob(Job j) {

                lock (lockObj)
                {
                    //verify if it's already over
                    LinkedListNode<long> node = listEndedBeforeAddingJob.Find(j.getJobId());
                    if (node != null)
                    {
                        listEndedBeforeAddingJob.Remove(node);
                        verifyIfShouldEnd();
                    }
                    else
                    {
                        dict.Add(j.getJobId(), j);
                    }
                }
            }

            public void removeJob(long id) {
                lock (lockObj) {
                    if (dict.ContainsKey(id)) {
                        dict.Remove(id);
                        verifyIfShouldEnd();
                    } else {
                        listEndedBeforeAddingJob.AddLast(id);
                    }
                }
            }

            private void verifyIfShouldEnd() {
                if (dict.Count == 0 && listEndedBeforeAddingJob.Count == 0) {
                    Monitor.Pulse(lockObj);
                }
            }

            public bool waitJobFinish(int timeout) {
                lock(lockObj) {
                    waitingEnd = true;
                    do {
                        if (dict.Count == 0) {
                            return true;
                        } 
                        //ver timeout correctamente
                        if (timeout == 0) {
                            waitingEnd = false;
                            return false;
                        }
                        try {
                            Monitor.Wait(lockObj, timeout);
                        } catch (ThreadInterruptedException e) {
                            Thread.CurrentThread.Interrupt();
                        }    
                    } while(true);
                }
            }
        }
        
        class clientEndJob : MarshalByRefObject, IEndJob
        {
            public void finish(long jobId)
            {
                Console.WriteLine("Mandou fechar o job com id " + jobId);
            }
        }

        static void Main()
        {
            Console.WriteLine("Client Started");

            string configFile = "Client.exe.config";
            RemotingConfiguration.Configure(configFile, false);
            WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
            IBrokerSAO brokerProxy = (IBrokerSAO)Activator.GetObject(entries[0].ObjectType, entries[0].ObjectUrl);

            //create jobManager
            JobManager jobManager = new JobManager();

            //Submissão do trabalho ao broker
            int numberOfJobs = 1000;
            for(int i=0;i<numberOfJobs;i++){
                Job j = new Job("service.exe", "inputFile"+i, "inputFile"+i+1, "clientEndPoint", new clientEndJob());
                long id = brokerProxy.SubmitJob(j);
                j.setJobId(id);
                
                jobManager.addJob(j);
            }

            Console.ReadLine();         
        }
    }
}
