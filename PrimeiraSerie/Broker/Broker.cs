using System;
using BrokerSAO;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Threading;
using JobImplementation;

namespace Broker
{
    static class Broker
    {
        static Dictionary<long, Job> dict;
        static long jobId = 0;
        static readonly Object genericLockObject = new Object();
        //static readonly string SERVER_EP = "JobBroker";


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
            return Interlocked.Increment(ref jobId);
        }

        static void Main()
        {
            ////Registo do canal
            //TcpChannel ch = new TcpChannel(1234);
            //ChannelServices.RegisterChannel(ch, false);

            ////Criação do objecto de submissão de Jobs            
            //MyBrokerObject myBrokerObject = new MyBrokerObject();
            //ObjRef brokerWellKnownObject = RemotingServices.Marshal((MarshalByRefObject) myBrokerObject, SERVER_EP);


            string configFile = "Broker.exe.config";
            RemotingConfiguration.Configure(configFile, false);

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

            Console.WriteLine("New Job Added (" + id + ")");
            Console.WriteLine(j.getJobDescription());
            
            j.setJobId(id);
            j.getEndJob().finish(j.getJobId());

            //Not thread safe
            Broker.getDictionary().Add(id, j);
            return id;
        }
    }
}