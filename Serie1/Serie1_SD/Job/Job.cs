using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IJob;

namespace JobImplementation
{
    [Serializable]
    public class Job:IJob.IJob
    {
        public long jobId;
        public string jobName { get; private set; }
        public string inputFilePath { get; private set; }
        public string outputFilePath { get; private set; }
        public string clientEndPoint { get; private set; }

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

        public static void Main(String[] args) { }
    }
}
