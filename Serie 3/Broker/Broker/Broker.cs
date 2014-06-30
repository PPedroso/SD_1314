using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrokerClientContract;
using BrokerStandContract;
using StandContract;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace BrokerInterface
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    partial class BrokerServices : IBrokerClientService
    {
        Broker broker = new Broker();


        public string helloWorld(string name) {
            return "Hello World " + name;
        }

        public void submitQueryByBrand(string uri,string brand) {
            Console.WriteLine(String.Format("{0} querying brand: {1}", uri, brand));
            broker.broadcastQuery(uri + " " + brand);
        }
    }

    partial class BrokerServices : IBrokerStandService
    {   
        public void registerStand(string standEndpoint)
        {
            EndpointAddress addr = new EndpointAddress(standEndpoint);
            WSHttpBinding bind = new WSHttpBinding();

            IChannelFactory<IStand> cfact = new ChannelFactory<IStand>(bind);
            IStand proxy = cfact.CreateChannel(addr);
            Console.WriteLine(String.Format("{0} registered", standEndpoint));
            broker.addStand(proxy);
        }
    }



    
    public class Broker
    {
        private LinkedList<IStand> stands = new LinkedList<IStand>();
        
        private const string BROKER_CLIENT_ENDPOINT = "http://localhost:8080/BrokerClientService" ;
        private const string BROKER_STAND_ENDPOINT = "http://localhost:8081/BrokerStandService";

        public void addStand(IStand proxy) {
            Console.WriteLine(String.Format("Broker adding stand: {0}",proxy.ToString()));
            stands.AddLast(proxy);
        }

        public void broadcastQuery(string query) {
            foreach (IStand p in stands) {
                Console.WriteLine(String.Format("Broadcasting {0}", p.ToString()));
                p.submitQuery(query);
            }
                
        }


        static void Main(string[] args)
        {

            Uri addrClient = new Uri(BROKER_CLIENT_ENDPOINT);
            Uri addrStand = new Uri(BROKER_STAND_ENDPOINT);

            Type serviceType = typeof(BrokerServices);
            BasicHttpBinding bind = new BasicHttpBinding();
            WSHttpBinding WSbind = new WSHttpBinding();

            ServiceHost svchost = new ServiceHost(serviceType);

            ServiceMetadataBehavior smb = svchost.Description.Behaviors.Find<ServiceMetadataBehavior>();

            if (smb != null) {
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = addrClient;
            }
            else{
                smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = addrClient;
                svchost.Description.Behaviors.Add(smb);
                ServiceDebugBehavior debug = svchost.Description.Behaviors.Find<ServiceDebugBehavior>();
                debug.IncludeExceptionDetailInFaults = true;
            }

            svchost.AddServiceEndpoint(typeof(IBrokerClientService), bind, addrClient);
            svchost.AddServiceEndpoint(typeof(IBrokerStandService), WSbind, addrStand);
            svchost.Open();
            Console.WriteLine("Broker service started, press any key to shut down");
            Console.ReadLine();
            svchost.Close();
            Console.WriteLine("Broker service closing");
            Console.ReadLine();

        }
    }
}
