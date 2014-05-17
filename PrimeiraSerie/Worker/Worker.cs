using System;
using WorkerSAO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using JobImplementation;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using BrokerCallback;
using System.Threading;
using System.Runtime.Remoting.Channels;

namespace Worker
{
    static class Worker
    {
        readonly static string SERVICE_BASE_PATH = "..\\..\\..\\Data\\Services\\";
        readonly static string INPUT_BASE_PATH = "..\\..\\..\\Data\\Input\\";
        readonly static string OUTPUT_BASE_PATH = "..\\..\\..\\Data\\Output\\";
        public static int port;
        public static volatile bool softCloseFlag = false;
        public static ManualResetEvent closeEvent = new ManualResetEvent(false);

        public static bool processJob(Job j)
        {
           ProcessStartInfo processInfo = new ProcessStartInfo(SERVICE_BASE_PATH + j.getJobName());
           processInfo.CreateNoWindow = true;
           processInfo.UseShellExecute = false; 
           processInfo.Arguments = INPUT_BASE_PATH + j.getInputFilePath() + " " + OUTPUT_BASE_PATH + j.getOutputFilePath();
           Process newProc = Process.Start(processInfo); 
           
           Console.WriteLine("Processing job:" + j.getJobId());
           
           newProc.WaitForExit();

           int exitcode = newProc.ExitCode;

           return (exitcode == 0); 
        }
        
        static void Main(string[] args)
        {
            string configFile = args[0];
            port = Int32.Parse(args[1]);
            RemotingConfiguration.Configure(configFile, false);

            Console.WriteLine("Worker started.");
            closeEvent.WaitOne();
        }
    }

    public class MyWorkerObject : MarshalByRefObject, IWorkerSAO
    {
        private volatile int currentJobs = 0; 

        
        public int getCurrentNumberOfJobs() {
            return currentJobs;
        }

        /*public void softClose(){
            Console.WriteLine("entrou");
            if (currentJobs == 0)
                closeWorker();
            else
            { 
                Console.WriteLine("fez set á flag");
                Worker.softCloseFlag = true;
            }
                

        }*/
        
        public void closeWorker(){
            Console.WriteLine("This just happened");
            Worker.closeEvent.Set();
        }

        public void incrementCurrentJobs() {
            Interlocked.Increment(ref currentJobs);
        }

        public void decrementCurrentJobs() { 
            Interlocked.Decrement(ref currentJobs);
        }

        public void submitJob(Job j ,IBrokerCallback callback)
        {
            Console.WriteLine("Job submited: " +j.getJobDescription());
            Task.Factory.StartNew(() => {
                Interlocked.Increment(ref currentJobs);

                if (Worker.processJob(j))
                    Console.WriteLine("Job " + j.getJobId() + " has finished with sucess");
                else
                    Console.WriteLine("Job " + j.getJobId() + " did not finish sucessfully");

                j.getEndJob().finish(j.getJobId());
                callback.finishJob(Worker.port, j.getJobId());
                Interlocked.Decrement(ref currentJobs);

                Console.WriteLine("flag = " + Worker.softCloseFlag + " currentJobs: " + currentJobs);
                if (Worker.softCloseFlag && (currentJobs == 0)) {
                    closeWorker();
                }
                    
            });            
        }
    }
}
