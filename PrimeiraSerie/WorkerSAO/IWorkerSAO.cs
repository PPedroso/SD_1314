using JobImplementation;
using BrokerCallback;
using System.Collections.Generic;

namespace WorkerSAO
{
    public interface IWorkerSAO
    {
        int getCurrentNumberOfJobs();
        void submitJob(KeyValuePair<Job,IBrokerCallback> kvp);
    }
}