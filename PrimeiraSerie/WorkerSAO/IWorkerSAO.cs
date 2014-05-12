using JobImplementation;

namespace WorkerSAO
{
    public interface IWorkerSAO
    {

        int getCurrentNumberOfJobs();
        void submitJob(Job jf);
    }
}