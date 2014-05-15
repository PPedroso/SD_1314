using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobImplementation;

namespace Broker
{
    public class JobWrapper
    {
        private Job j;
        private status jobStatus;

        enum status { QUEUED, RUNNING, FINISHED, FAILED };

        public JobWrapper(Job j)
        {
            this.j = j;
            jobStatus = status.QUEUED;
        }

        public Job getJob()
        {
            return j;
        }

        public void setJobFailed() { jobStatus = status.FAILED; }

        public void setJobStatusRunning() { jobStatus = status.RUNNING; }

        public void setJobStatusFinished() { jobStatus = status.FINISHED; }

        public string getJobStatus() { return jobStatus.ToString(); }

    }
}
