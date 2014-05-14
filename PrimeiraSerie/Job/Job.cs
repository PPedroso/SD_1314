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
        private string clientEndPoint;
        private IEndJob endJob;


        public Job(string jobName, string inputFilePath, string outputFilePath, string clientEndPoint, IEndJob endJob)
        {
            this.jobName = jobName;
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
            this.clientEndPoint = clientEndPoint;
            this.endJob = endJob;
        }

        public string getJobDescription()
        {
            return " JobId:" + jobId +
                   "\n Service:" + jobName +
                   "\n InputFile:" + inputFilePath +
                   "\n OutputFile:" + outputFilePath +
                   "\n clientEndPoint:" + clientEndPoint;
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

        public String getClientEndPoint()
        {
            return clientEndPoint;
        }

        public IEndJob getEndJob() 
        {
            return endJob;
        }
    }
}
