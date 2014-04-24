using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
   interface IBroker
   {
       /// <summary>
       /// 
       /// </summary>
       /// <param name="jobId"></param>
       /// <returns>Whether the job is still being processed</returns>
       bool RequestJobStatus(int jobId);
       int SubmitJob(JobForm jf);
   }
}
