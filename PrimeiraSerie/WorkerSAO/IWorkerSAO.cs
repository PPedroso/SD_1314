using JobImplementation;
using BrokerCallback;

namespace WorkerSAO
{
    public interface IWorkerSAO
    {
        int getCurrentNumberOfJobs();
        void submitJob(Job j,IBrokerCallback callback);
        void softClose();
    }
}