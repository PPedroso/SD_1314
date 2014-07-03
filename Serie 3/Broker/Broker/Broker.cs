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
        private readonly Object lockObj = new Object();
        private List<IStandBrokerContract> stands = new List<IStandBrokerContract>();

        public void addStand(IStandBrokerContract proxy) {
            Console.WriteLine(String.Format("Broker adding stand: {0}",proxy.ToString()));
            lock (lockObj) {
                stands.Add(proxy);
            }
        }

        public void broadcastBrandQuery(string client, string brand) { 
            broadcast(client, stand => {
                try {
                    stand.queryByBrand(client, brand);
                    return false;
                } catch (Exception e) {
                    Console.WriteLine("Removing one of the stands, since it failed to propagate brand query");
                    return true;
                }
            }); 
        }

        public void broadcastYearQuery(string client, int year) {
            broadcast(client, stand => {
                try {
                    stand.queryByMinimumYearRegistration(client, year);
                    return false;
                } catch (Exception e) {
                    Console.WriteLine("Removing one of the stands, since it failed to propagate year query");
                    return true;
                }
            });
        }

        public void broadcastPriceQuery(string client, int price) {
            broadcast(client, stand => {
                try {
                    stand.queryByMaxPrice(client, price);
                    return false;
                } catch (Exception e) {
                    Console.WriteLine("Removing one of the stands, since it failed to propagate price query");
                    return true;
                }
            });
        }

        private void broadcast(string client, Func<IStandBrokerContract, bool> func) {
            lock(lockObj) {
                for (int i = 0; i < stands.Count; i++) {
                    if (func(stands[i])) {
                        //in case it needs to be removed
                        stands.RemoveAt(i);
                    }
                }
            }
        }

        static void Main(string[] args) {
            using (ServiceHost host = new ServiceHost(typeof(BrokerServices))) {
                host.Open();
                Console.WriteLine("Broker service started, press any key to shut down");
                Console.ReadLine();
            }
        }
    }
}
