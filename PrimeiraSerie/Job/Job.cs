using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobImplementation
{
    public interface IEndJob 
    {
       void finish(long jobId);
    }

    [Serializable]
    public class Job
    {
        private long jobId;
        private string jobName;
        private string inputFilePath;
        private string outputFilePath;
        private IEndJob endJob;


        public Job(string jobName, string inputFilePath, string outputFilePath, IEndJob endJob)
        {
            this.jobName = jobName;
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
            this.endJob = endJob;
        }

        public string getJobDescription()
        {
            return "\n JobId:" + jobId +
                   "\n Service:" + jobName +
                   "\n InputFile:" + inputFilePath +
                   "\n OutputFile:" + outputFilePath;
        }

        public void setJobId(long jobId)
        {
            this.jobId = jobId;
        }

        public long getJobId()
        {
            return jobId;
        }

        public String getInputFilePath()
        {
            return inputFilePath;
        }

        public String getOutputFilePath()
        {
            return outputFilePath;
        }

        public String getJobName()
        {
            return jobName;
        }

        public IEndJob getEndJob() 
        {
            return endJob;
        }
    }
}
