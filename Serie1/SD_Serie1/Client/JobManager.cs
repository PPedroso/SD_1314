using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientJob;

namespace Client
{
    [Serializable]
    class JobManager
    {
        Dictionary<Int32, IJob> dict = new Dictionary<Int32, IJob>();

        public void addJob(Int32 identifier, IJob job) {
            dict.Add(identifier, job);
        }

        public void removeJob(Int32 identifier) {
            dict.Remove(identifier);
        }
    }
}
