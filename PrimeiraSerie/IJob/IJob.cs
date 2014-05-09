using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IJobNS
{
    public interface IJob
    {
        void setJobId(long id);
        long getJobId();
        string getJobDescription();
        string getInputFilePath();
        string getOutputFilePath();
        string getJobName();
    }
}
