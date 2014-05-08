
using System.Collections.Generic;

namespace IWorker
{
    public interface IWorker
    {
        List<IJob.IJob> getJobs();
        bool executeJob(IJob.IJob job);
    }
}
