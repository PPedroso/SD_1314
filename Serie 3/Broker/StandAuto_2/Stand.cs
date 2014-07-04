using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using StandContract;
using StandClientContract;
using ClientContract;

namespace StandAuto
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    partial class Stand : IStandBrokerContract {

        private Object lockObj = new Object();
        private CarsSingleton carsSingleton = CarsSingleton.getInstance();

        public void queryByBrand(string client, String brand) {
            try {
                submitProposals(client, carsSingleton.getCarsWithBrand(brand));
            } catch (Exception e) {
                //no need to deal with exception
                Console.WriteLine(String.Format("Error during query by brand for client {0}: {1}", client, e.Message));
            }
        }

        public void queryByMaxPrice(string client, int maxPrice) {
            try {
                submitProposals(client, carsSingleton.getCarsWithPriceLowerThen(maxPrice));
            } catch (Exception e) {
                //no need to deal with exception
                Console.WriteLine(String.Format("Error during query by price for client {0}: {1}", client, e.Message));
            }
        }

        public void queryByMinimumYearRegistration(string client, int minYearRegistration) {
            try {
                submitProposals(client, carsSingleton.getCarsWithYearRegistrationHigherThen(minYearRegistration));
            } catch (Exception e) {
                //no need to deal with exception
                Console.WriteLine(String.Format("Error during query by year for client {0}: {1}", client, e.Message));
            }
        }

        private void submitProposals(String client, IEnumerable<Car> cars) {
            
            EndpointAddress addr = new EndpointAddress(client);
            BasicHttpBinding bind = new BasicHttpBinding();

            IChannelFactory<IClient> cfact = new ChannelFactory<IClient>(bind);
            IClient proxy = cfact.CreateChannel(addr);
            
            
            
            foreach (Car c in cars) {
                proxy.submitProposal(Program.STAND_CLIENT_SERVICE_ENDPOINT, c.getId(), c.getBrand(), c.getYearRegistration(), c.getPrice());
            }
        }
    }

    partial class Stand : IStandClientContract {
        public void reserveCar(int id) {
            if (!carsSingleton.setReservedStatus(id, true)) {
                throw new FaultException<AlreadyReservedFault>(new AlreadyReservedFault { Id = id });
            }
        }
    }
}
