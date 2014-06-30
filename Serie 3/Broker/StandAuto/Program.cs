using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using BrokerStandContract;
using StandContract;

namespace StandAuto
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode= ConcurrencyMode.Multiple)]
    public class Stand : IStand {
        public void submitQuery(string query) {
            Console.WriteLine(String.Format("Querying stand with query: {0}", query));
        }
    }
    
    class Program
    {

        const string BASE_HTTP = "http://localhost:";
        const string BROKER_SERVICE_ENDPOINT = "http://localhost:8081/BrokerStandService";
        static string STAND_SERVICE_ENDPOINT;

        static ServiceHost svchost;

        private static void registerStandInBroker(){
            EndpointAddress addr = new EndpointAddress(BROKER_SERVICE_ENDPOINT);
            WSHttpBinding bind = new WSHttpBinding();
            
            IBrokerStandService proxy;

            IChannelFactory<IBrokerStandService> cfact = new ChannelFactory<IBrokerStandService>(bind);
            proxy = cfact.CreateChannel(addr);

            proxy.registerStand(STAND_SERVICE_ENDPOINT);
        }

        private static void startService(string port) {

            STAND_SERVICE_ENDPOINT = BASE_HTTP + port + "/StandService";

            Uri addr = new Uri(STAND_SERVICE_ENDPOINT);
            Type servType = typeof(StandAuto.Stand);
            svchost = new ServiceHost(servType);
            WSHttpBinding bind = new WSHttpBinding();
            bind.Security.Mode = SecurityMode.Message;

            ServiceMetadataBehavior smb = svchost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (smb != null)
            {
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = addr;

            }
            else
            {
                smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = addr;
                svchost.Description.Behaviors.Add(smb);
            }

            svchost.AddServiceEndpoint(typeof(IStand), bind, addr);
            svchost.Open();

          
        }
        
        
        static void Main(string[] args)
        {
            Console.WriteLine("Insert port");
            string port = Console.ReadLine();


            startService(port);
            registerStandInBroker();

            Console.WriteLine("Stand registered in broker");
            Console.WriteLine("Press any key to shutdown stand");
            Console.ReadLine();
            svchost.Close();
            Console.ReadLine();

            
        }
    }
}
