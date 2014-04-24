using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace Server
{
    static class Server
    {
        static Dictionary<Object, Object> dict;
        static readonly Object genericLockObject = new Object();

        public static Dictionary<Object, Object> getDictionary()
        {
            if (dict == null)
            {
                lock (genericLockObject)
                {
                    if (dict == null)
                    {
                        dict = new Dictionary<Object, Object>();
                    }
                }
            }
            return dict;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
             */

            RemotingConfiguration.RegisterWellKnownServiceType(typeof(Broker), "JobBrokering.soap", WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterActivatedServiceType(typeof(IBroker));
            Console.WriteLine("Broker Is Brokering. Press any key to unbrokerate.");
            Console.ReadLine();
        }
    }

    public class Broker : IBroker
    {
        public bool RequestJobStatus(int jobId)
        {
            Dictionary<Object, Object> thatOneDictionary = Server.getDictionary();
            throw new NotImplementedException();
        }

        public int SubmitJob(JobForm jf)
        {
            throw new NotImplementedException();
        }
    }
}