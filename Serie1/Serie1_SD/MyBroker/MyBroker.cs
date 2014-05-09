using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using IJobNS;

namespace IBrokerCAO
{
    static class MyBroker
    {
        static Dictionary<long, IJob> dict;
        static long jobId = 0;
        static readonly Object genericLockObject = new Object();
        static readonly string SERVER_EP = "JobBroker";


        public static Dictionary<long, IJob> getDictionary()
        {
            if (dict == null)
            {
                lock (genericLockObject)
                {
                    if (dict == null)
                    {
                        dict = new Dictionary<long, IJob>();
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


            string configFile = "MyBroker.exe.config";
            RemotingConfiguration.Configure(configFile, false);

            Console.WriteLine("Broker is working, press any key to shut down");
            Console.ReadLine();
        }
    }

    public class MyBrokerObject : MarshalByRefObject,IMyBrokerCAO
    {
        public bool RequestJobStatus(long jobId)
        {
            Dictionary<long, IJob> myDict = MyBroker.getDictionary();
            throw new NotImplementedException();
        }

        public long SubmitJob(IJob j)
        {
            long id = MyBroker.getCurrentJobId();
            Dictionary<long, IJob> myDict = MyBroker.getDictionary();
            // = id;
            
            Console.WriteLine("New Job Added (" + id + ")");
            Console.WriteLine(j.getJobDescription());
            
            //Not thread safe
            MyBroker.getDictionary().Add(id, j);
            return id;
        }
    }
}