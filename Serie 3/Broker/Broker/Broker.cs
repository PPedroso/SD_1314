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
            using (ServiceHost host = new ServiceHost(typeof(BrokerServices))) {
                host.Open();
                Console.WriteLine("Broker service started, press any key to shut down");
                Console.ReadLine();
            }
        }

    }

    public static class MyExtensions {
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action) {
            foreach (var x in @this) { action(x); }
        }
    }
}
