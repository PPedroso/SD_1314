using IJobNS;

namespace IBrokerSAO
{
    public interface IMyBrokerSAO
    {
        bool RequestJobStatus(long jobId);
        long SubmitJob(IJob jf);
    }
}
