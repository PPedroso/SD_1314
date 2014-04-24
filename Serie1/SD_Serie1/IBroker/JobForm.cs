using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBroker
{
    [Serializable]
    class JobForm
    {
        public int  jobId
        public string jobName           { get; private set; }
        public string inputFilePath     { get; private set; }
        public string outputFilePath    { get; private set; }
        public string clientEndPoint    { get; private set; }

        public JobForm(string jobName, string inputFilePath, string outputFilePath, string clientEndPoint)
        {
            this.jobName        = jobName;
            this.inputFilePath  = inputFilePath;
            this.outputFilePath = outputFilePath;
            this.clientEndPoint = clientEndPoint;
        }
    }
}
