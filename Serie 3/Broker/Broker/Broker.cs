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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    partial class BrokerServices : IBrokerClientService
    {
        Broker broker = new Broker();

        public string helloWorld(string name) {
            return "Hello World " + name;
        }

        public void submitQueryByBrand(string client, string brand) {
            try
            {
                Console.WriteLine(String.Format("{0} querying brand: {1}", client, brand));
                broker.broadcastBrandQuery(client, brand);
            }
            catch (Exception e) {
                Console.WriteLine("Error:" + e.Message);
            }
            
        }

        public void submitQueryByMinumumYearRegistration(string client, int yearRegistration) {
            Console.WriteLine(String.Format("{0} querying year registration: {1}", client, yearRegistration));
            broker.broadcastYearQuery(client, yearRegistration);
        }

        public void submitQueryByMaxPrice(string client, int maxPrice) {
            Console.WriteLine(String.Format("{0} querying brand: {1}", client, maxPrice));
            broker.broadcastPriceQuery(client, maxPrice);
        }
    }

    partial class BrokerServices : IBrokerStandService
    {   
        public void registerStand(string standEndpoint)
        {
            EndpointAddress addr = new EndpointAddress(standEndpoint);
            WSHttpBinding bind = new WSHttpBinding();
            bind.Security.Mode = SecurityMode.Message;

            IChannelFactory<IStandBrokerContract> cfact = new ChannelFactory<IStandBrokerContract>(bind);
            IStandBrokerContract proxy = cfact.CreateChannel(addr);
            Console.WriteLine(String.Format("{0} registered", standEndpoint));
            broker.addStand(proxy);
        }
    }

    public class Broker
    {
        private LinkedList<IStandBrokerContract> stands = new LinkedList<IStandBrokerContract>();
        
        private const string BROKER_CLIENT_ENDPOINT = "http://localhost:8080/BrokerClientService" ;
        private const string BROKER_STAND_ENDPOINT = "http://localhost:8081/BrokerStandService";

        public void addStand(IStandBrokerContract proxy) {
            Console.WriteLine(String.Format("Broker adding stand: {0}",proxy.ToString()));
            stands.AddLast(proxy);
        }

        public void broadcastBrandQuery(string client, string brand) { 
            stands.ForEach(stand => stand.queryByBrand(client, brand)); 
        }

        public void broadcastYearQuery(string client, int year) {
            stands.ForEach(stand => stand.queryByMinimumYearRegistration(client, year)); 
        }

        public void broadcastPriceQuery(string client, int price) {
            stands.ForEach(stand => stand.queryByMaxPrice(client, price)); 
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

    public static class MyExtensions {
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action) {
            foreach (var x in @this) { action(x); }
        }
    }
}
