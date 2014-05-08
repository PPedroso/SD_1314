using JobImplementation;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Threading;

namespace IBrokerCAO
{
    static class MyBroker
    {
        static Dictionary<long, Job> dict;
        static long jobId = 0;
        static readonly Object genericLockObject = new Object();
        static readonly string SERVER_EP = "JobBrokering.soap";


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

            HttpChannel ch = new HttpChannel(1234);
            ChannelServices.RegisterChannel(ch, false);

            RemotingConfiguration.RegisterWellKnownServiceType(typeof(MyBrokerCAO), SERVER_EP, WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterActivatedServiceType(typeof(IMyBrokerCAO));
            Console.WriteLine("Broker Is Brokering. Press any key to unbrokerate.");
            Console.ReadLine();
        }
    }

    public class MyBrokerCAO : MarshalByRefObject,IMyBrokerCAO
    {
        public bool RequestJobStatus(long jobId)
        {
            Dictionary<long, Job> myDict = MyBroker.getDictionary();
            throw new NotImplementedException();
        }

        public long SubmitJob(Job j)
        {
            long id = MyBroker.getCurrentJobId();
            Dictionary<long, Job> myDict = MyBroker.getDictionary();
            j.jobId = id;
            
            Console.WriteLine("New Job Added (" + id + ")");
            Console.WriteLine(j.getJobDescription());
            
            //Not thread safe
            MyBroker.getDictionary().Add(id, j);
            return id;
        }
    }
}