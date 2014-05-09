using JobImplementation;

namespace BrokerSAO
{
    public interface IBrokerSAO
    {
        bool RequestJobStatus(long jobId);
        long SubmitJob(Job jf);
    }
}
