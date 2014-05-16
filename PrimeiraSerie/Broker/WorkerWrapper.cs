using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerSAO;
using JobImplementation;
using System.Threading;

namespace Broker
{
    public class WorkerWrapper
    {
        private IWorkerSAO workerProxy;
        private int currentJobs=0;
        private LinkedList<Job> jobList = new LinkedList<Job>();


        public WorkerWrapper(IWorkerSAO proxy) {
            workerProxy = proxy;
        }
        
        public IWorkerSAO getProxy() { return workerProxy; }
        public int getCurrentJobs() { return currentJobs; }
        public LinkedList<Job> getJobList() { return jobList; }
        
        public void addJob(Job j) {
            jobList.AddLast(j);
            ++currentJobs;
            try
            {
                workerProxy.submitJob(j, new MyBrokerCallbackObject());
            }
            catch (Exception e) {
                Console.WriteLine("Error message: " + e.Message);
            }
            
        }

        public void removeJob(long jobId) {
            foreach (Job j in jobList){
                if (j.getJobId() == jobId)
                    jobList.Remove(j);
            }
        }
    }
}
