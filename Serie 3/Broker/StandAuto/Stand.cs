using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StandContract;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;

namespace StandAuto
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Stand : IStand
    {
        CarsSingleton carsSingleton = CarsSingleton.getInstance();

        public void queryByBrand(string client, String brand) {
            IEnumerable<Car> result = carsSingleton.getCarsWithBrand(brand);
        }

        public void queryByMaxPrice(string client, int maxPrice) {
            IEnumerable<Car> result = carsSingleton.getCarsWithPriceLowerThen(maxPrice);
        }

        public void queryByMinimumYearRegistration(string client, int minYearRegistration) {
            IEnumerable<Car> result = carsSingleton.getCarsWithYearRegistrationHigherThen(minYearRegistration);
        }
    }
}
