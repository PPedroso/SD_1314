using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using StandContract;
using StandClientContract;
using StandAuto.CLIENT;

namespace StandAuto
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    partial class Stand : IStandBrokerContract
    {
        CarsSingleton carsSingleton = CarsSingleton.getInstance();

        public void queryByBrand(string client, String brand) {
            try
            {
                IEnumerable<Car> result = carsSingleton.getCarsWithBrand(brand);
                ClientContract proxy = new ClientContractClient();
                
                foreach(Car c in result) {
                    proxy.submitProposal(Program.STAND_BROKER_SERVICE_ENDPOINT, String.Format("Car({0},{1},{2})",c.getId(),c.getBrand(),c.getYearRegistration()));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
        }

        public void queryByMaxPrice(string client, int maxPrice) {
            IEnumerable<Car> result = carsSingleton.getCarsWithPriceLowerThen(maxPrice);
            foreach (Car c in result) { Console.WriteLine(c.getPrice()); }
        }

        public void queryByMinimumYearRegistration(string client, int minYearRegistration) {
            IEnumerable<Car> result = carsSingleton.getCarsWithYearRegistrationHigherThen(minYearRegistration);
            foreach (Car c in result) { Console.WriteLine(c.getYearRegistration()); }
        }
    }

    partial class Stand : IStandClientContract 
    {
        public void reserveCar(int id)
        {
            Console.WriteLine("Car with id " + id + " reserved");
        }
    }
}
