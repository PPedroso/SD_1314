using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IJobNS;

namespace JobImplementation
{
    [Serializable]
    public class Job : IJob
    {
        private long jobId;
        private string jobName;
        private string inputFilePath;
        private string outputFilePath;
        private string clientEndPoint;

        public Job(string jobName, string inputFilePath, string outputFilePath, string clientEndPoint)
        {
            this.jobName = jobName;
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
            this.clientEndPoint = clientEndPoint;
        }

        public string getJobDescription()
        {
            return "JobId:" + jobId +
                   "\n Service:" + jobName +
                   "\n InputFile:" + inputFilePath +
                   "\n OutputFile:" + outputFilePath +
                   "\n clientEndPoint:" + clientEndPoint;
        }

        public void setJobId(long jobId) {
            this.jobId = jobId;
        }

        public long getJobId() {
            return jobId;
        }
        
        public String getInputFilePath() {
            return inputFilePath;
        }

        public String getOuputFilePath() {
            return outputFilePath;
        }

        public String getJobName() {
            return jobName;
        }

        public String getClientEndPoint() {
            return clientEndPoint;
        }

    }
}
