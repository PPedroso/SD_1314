﻿using System;
using WorkerSAO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using JobImplementation;
using System.Threading;
using System.Diagnostics;
using System.IO;
using BrokerCallback;

namespace Worker
{
    static class Worker
    {
        readonly static string SERVICE_BASE_PATH = "C:\\Users\\Pedro\\Escola\\SD\\Series\\PrimeiraSerie\\Data\\Services\\";
        readonly static string INPUT_BASE_PATH = "C:\\Users\\Pedro\\Escola\\SD\\Series\\PrimeiraSerie\\Data\\Input\\";
        readonly static string OUTPUT_BASE_PATH = "C:\\Users\\Pedro\\Escola\\SD\\Series\\PrimeiraSerie\\Data\\Output\\";

        public static void processJob(Job j)
        {
           ProcessStartInfo processInfo = new ProcessStartInfo(SERVICE_BASE_PATH + j.getJobName());
           processInfo.CreateNoWindow = true;
           processInfo.UseShellExecute = false; 
           processInfo.Arguments = INPUT_BASE_PATH + j.getInputFilePath() + " " + OUTPUT_BASE_PATH + j.getOutputFilePath();
           Process newProc = Process.Start(processInfo); 
           
           Console.WriteLine("Processing job:" + j.getJobId());
           
           newProc.WaitForExit();
           
           int exitcode = newProc.ExitCode;
           if (exitcode == 0)
           {
               Console.WriteLine("Job " + j.getJobId() + " has finished with sucess");
               j.getEndJob().finish(j.getJobId());
           }
           else {
               Console.WriteLine("Job " + j.getJobId() + " did not finish sucessfully");
               j.getEndJob().finish(exitcode);
           }

           
        }
        
        static void Main(string[] args)
        {
            string configFile = args[0];
            RemotingConfiguration.Configure(configFile, false);

            Console.WriteLine("I am a worker and I am working. Press any key for me to stop working...");
            Console.ReadLine();

        }
    }

    public class MyWorkerObject : MarshalByRefObject, IWorkerSAO
    {
        private volatile int currentJobs = 0;
        
        public int getCurrentNumberOfJobs() {
            return currentJobs;
        }

        public void incrementCurrentJobs() {
            Interlocked.Increment(ref currentJobs);
        }

        public void decrementCurrentJobs() { 
            Interlocked.Decrement(ref currentJobs);
        }

        public void submitJob(KeyValuePair<Job,IBrokerCallback> k)
        {
            Job j = k.Key;
            Console.WriteLine("Job submited: " +j.getJobDescription());
            Interlocked.Increment(ref currentJobs);
            Worker.processJob(j);
            k.Value.finishJob(j.getJobId());
            Interlocked.Decrement(ref currentJobs);
        }
    }
}
