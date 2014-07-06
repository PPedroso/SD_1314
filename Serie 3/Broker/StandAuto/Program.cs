using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.IO;
using System.Xml.Linq;

using StandContract;
using StandClientContract;
using StandAuto.Broker;


namespace StandAuto
{    
    class Program
    {
        const string BROKER_SERVICE_ENDPOINT = "http://localhost:8080/BrokerStandService";
        public static string STAND_BROKER_SERVICE_ENDPOINT;
        public static string STAND_CLIENT_SERVICE_ENDPOINT;
        

        static ServiceHost svchost;

        private static void registerStandInBroker(){
            STAND_BROKER_SERVICE_ENDPOINT = svchost.Description.Endpoints[0].Address.ToString();
            

            BrokerStandClient proxy = new BrokerStandClient();

            proxy.registerStand(STAND_BROKER_SERVICE_ENDPOINT);
        }

        private static void startStandBrokerService() {
            svchost = new ServiceHost(typeof(Stand));
            svchost.Open();
            STAND_BROKER_SERVICE_ENDPOINT = svchost.Description.Endpoints[0].Address.ToString();
            STAND_CLIENT_SERVICE_ENDPOINT = svchost.Description.Endpoints[1].Address.ToString();
        }

        static void Main(string[] args)
        {    

            CarsSingleton.getInstance().initiate(@"..\..\database.xml");
            startStandBrokerService();
            registerStandInBroker();

            Console.WriteLine("Stand registered in broker");
            Console.WriteLine("Press any key to shutdown stand");
            Console.ReadLine();
            svchost.Close();
            Console.ReadLine();

            
        }
    }
}
