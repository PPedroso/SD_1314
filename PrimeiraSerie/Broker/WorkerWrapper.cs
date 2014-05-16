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
        private Dictionary<long, Job> dictJobs = new Dictionary<long, Job>();
        private bool isFunctioning = true;
        private readonly int port;

        public WorkerWrapper(IWorkerSAO proxy, int port) {
            workerProxy = proxy;
            this.port = port;
        }
        
        public IWorkerSAO getWorkerSAO() { return workerProxy; }
        public int getCurrentJobs() { return currentJobs; }
        public int getPort() { return port; }
        public bool getIsFunctioning() { return isFunctioning; }
        public void setIsFunctioning(bool isFunctioning) { this.isFunctioning = isFunctioning; }


        //usado apenas quando o broker foi "abaixo", por isso nao a prob de excepções
        public IEnumerable<Job> getJobList() {
            return dictJobs.Select((x) => x.Value); 
        }
        
        public void addJob(Job j) {
            dictJobs.Add(j.getJobId(), j);
            ++currentJobs;
            workerProxy.submitJob(j, new MyBrokerCallbackObject());
        }

        public void removeJob(long jobId) {
            dictJobs.Remove(jobId);
            --currentJobs;
        }
    }
}
