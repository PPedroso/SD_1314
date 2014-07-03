using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using BrokerStandContract;
using StandContract;
using StandClientContract;
using System.IO;
using System.Xml.Linq;

namespace StandAuto
{    
    class Program
    {

        const string BASE_HTTP = "http://localhost:";
        const string BROKER_SERVICE_ENDPOINT = "http://localhost:8081/BrokerStandService";
        public static string STAND_BROKER_SERVICE_ENDPOINT;
        public static string STAND_CLIENT_SERVICE_ENDPOINT;
        

        static ServiceHost svchost;

        private static void registerStandInBroker(){
            EndpointAddress addr = new EndpointAddress(BROKER_SERVICE_ENDPOINT);
            WSHttpBinding bind = new WSHttpBinding();
            
            IBrokerStandService proxy;

            IChannelFactory<IBrokerStandService> cfact = new ChannelFactory<IBrokerStandService>(bind);
            proxy = cfact.CreateChannel(addr);

            proxy.registerStand(STAND_BROKER_SERVICE_ENDPOINT);
        }

        private static void startStandBrokerService(string port) {

            STAND_BROKER_SERVICE_ENDPOINT = BASE_HTTP + port + "/StandBrokerService";
            STAND_CLIENT_SERVICE_ENDPOINT = BASE_HTTP + port + "/StandClientService";

            Uri wsAddr = new Uri(STAND_BROKER_SERVICE_ENDPOINT);
            Uri httpAddr = new Uri(STAND_CLIENT_SERVICE_ENDPOINT);
            Type servType = typeof(StandAuto.Stand);
            svchost = new ServiceHost(servType);
            WSHttpBinding wsBind = new WSHttpBinding();
            BasicHttpBinding httpBind = new BasicHttpBinding();
            
            wsBind.Security.Mode = SecurityMode.Message;

            ServiceMetadataBehavior smb = svchost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (smb != null)
            {
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = wsAddr;

            }
            else
            {
                smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = wsAddr;
                svchost.Description.Behaviors.Add(smb);
            }

            svchost.AddServiceEndpoint(typeof(IStandBrokerContract), wsBind, wsAddr);
            svchost.AddServiceEndpoint(typeof(IStandClientContract), httpBind, httpAddr);
            svchost.Open();
          
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Insert port");
            string port = Console.ReadLine();

            CarsSingleton.getInstance().initiate(String.Format(@"..\..\{0}.xml", port));
            startStandBrokerService(port);
            registerStandInBroker();

            Console.WriteLine("Stand registered in broker");
            Console.WriteLine("Press any key to shutdown stand");
            Console.ReadLine();
            svchost.Close();
            Console.ReadLine();

            
        }
    }
}
