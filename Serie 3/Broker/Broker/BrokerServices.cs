using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

using BrokerClientContract;
using BrokerStandContract;
using System.ServiceModel.Channels;
using StandContract;

namespace BrokerInterface
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    partial class BrokerServices : IBrokerClientService
    {
        Broker broker = new Broker();

        public string helloWorld(string name)
        {
            return "Hello World " + name;
        }

        public void submitQueryByBrand(string client, string brand)
        {
            try
            {
                Console.WriteLine(String.Format("{0} querying brand: {1}", client, brand));
                broker.broadcastBrandQuery(client, brand);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }

        }

        public void submitQueryByMinumumYearRegistration(string client, int yearRegistration)
        {
            Console.WriteLine(String.Format("{0} querying year registration: {1}", client, yearRegistration));
            broker.broadcastYearQuery(client, yearRegistration);
        }

        public void submitQueryByMaxPrice(string client, int maxPrice)
        {
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
}
