using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobImplementation;
using WorkerSAO;

namespace Broker
{
    public class JobWrapper
    {
        private Job j;
        private status jobStatus;
        private WorkerWrapper worker;

        enum status { QUEUED, RUNNING, FINISHED};

        public WorkerWrapper getWorkerWrapper()
        {
            return worker;
        }

        public void setWorkerWrapper(WorkerWrapper worker)
        {
            this.worker = worker;
        }
        
        public JobWrapper(Job j)
        {
            this.j = j;
            jobStatus = status.QUEUED;
        }

        public Job getJob()
        {
            return j;
        }

        public void setJobStatusRunning() { jobStatus = status.RUNNING; }

        public void setJobStatusFinished() { jobStatus = status.FINISHED; }

        public string getJobStatus() { return jobStatus.ToString(); }

    }
}
