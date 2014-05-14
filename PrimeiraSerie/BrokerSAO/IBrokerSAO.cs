using JobImplementation;

namespace BrokerSAO
{
    public interface IBrokerSAO
    {
        string RequestJobStatus(long jobId);
        long SubmitJob(Job jf);
    }
}
