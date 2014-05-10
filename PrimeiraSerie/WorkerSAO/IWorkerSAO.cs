using JobImplementation;

namespace WorkerSAO
{
    public interface IWorkerSAO
    {
        bool ResquestJobStatus();
        void submitJob(Job jf);
    }
}