using System;
using BrokerSAO;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Threading;
using JobImplementation;
using System.IO;
using System.Diagnostics;

namespace Broker
{
    static class Broker
    {
        static Dictionary<long, Job> dict;
        static long jobId = 0;
        static int baseWorkerPort = 2000;
        static int numberOfWorkers=0;
        static readonly Object genericLockObject = new Object();

        public static Dictionary<long, Job> getDictionary()
        {
            if (dict == null)
            {
                lock (genericLockObject)
                {
                    if (dict == null)
                    {
                        dict = new Dictionary<long, Job>();
                    }
                }
            }
            return dict;
        }

        public static long getCurrentJobId()
        {
            long nId = Interlocked.Increment(ref jobId);
            return nId;
        }

        static void initWorkers(){
            string configFilePath = createConfigFile();
            ProcessStartInfo processInfo= new ProcessStartInfo(
                "C:\\Users\\Pedro\\Escola\\SD\\Series\\PrimeiraSerie\\Worker\\bin\\Debug\\Worker.exe");
            processInfo.Arguments = configFilePath;            Process newProc = Process.Start(processInfo); 
        }

        
        private static string createConfigFile(){
            StreamWriter file = new StreamWriter("WorkerConfig" + ++numberOfWorkers + ".exe.config");
            file.WriteLine(@"<?xml version=""1.0""?>");
            file.WriteLine("<configuration>");
            file.WriteLine("<system.runtime.remoting>");
            file.WriteLine("<application>");
            file.WriteLine("<channels>");
            file.WriteLine(@"<channel ref=""tcp"" port=""" + (baseWorkerPort+numberOfWorkers) + @""">");
            file.WriteLine("<clientProviders>");
            file.WriteLine(@"<formatter ref=""binary""/>");
            file.WriteLine("</clientProviders>");
            file.WriteLine("<serverProviders>");
            file.WriteLine(@"<formatter ref=""binary"" typeFilterLevel=""Full""/>");
            file.WriteLine("</serverProviders>");
            file.WriteLine("</channel>");
            file.WriteLine("</channels>");
            file.WriteLine("<service>");
            file.WriteLine(@"<wellknown type=""Worker.MyWorkerObject, Worker"" objectUri=""JobBroker"" mode=""Singleton""/>");
            file.WriteLine("</service>");
            file.WriteLine("</application>");
            file.WriteLine("</system.runtime.remoting>");
            file.WriteLine("</configuration>");
            file.Close();
            return Path.GetFullPath("WorkerConfig" +numberOfWorkers + ".exe.config");
        }
        
        static void Main()
        {
          
            string brokerClientConfigFile = "Broker.exe.config";
            RemotingConfiguration.Configure(brokerClientConfigFile, false);
            initWorkers();
            Console.WriteLine("Broker is working, press any key to shut down");
            Console.ReadLine();
        }
    }

    public class MyBrokerObject : MarshalByRefObject, IBrokerSAO
    {
        public bool RequestJobStatus(long jobId)
        {
            Dictionary<long, Job> myDict = Broker.getDictionary();
            throw new NotImplementedException();
        }

        public long SubmitJob(Job j)
        {
            long id = Broker.getCurrentJobId();
            Dictionary<long, Job> myDict = Broker.getDictionary();

            j.setJobId(id);

            Console.WriteLine("New Job Added (" + id + ")");
            Console.WriteLine(j.getJobDescription());

            j.getEndJob().finish(j.getJobId());

            //Not thread safe
            
            myDict.Add(id, j);
            return id;
        }
    }
}