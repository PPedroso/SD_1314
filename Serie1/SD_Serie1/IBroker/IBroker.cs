using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
   interface IBroker
   {
       JobStatus RequestJobStatus(int jobId);
       int SubmitJob(JobForm jf);
   }
}
