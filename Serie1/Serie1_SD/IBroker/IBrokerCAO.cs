using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobImplementation;

namespace IBrokerCAO
{
    public interface IMyBrokerCAO
    {
        bool RequestJobStatus(long jobId);
        long SubmitJob(Job jf);
    }
}
