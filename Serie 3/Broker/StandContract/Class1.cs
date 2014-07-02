using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace StandContract
{
    [ServiceContract(Namespace = "http://ISEL.STAND.BROKER")]
    public interface IStandBrokerContract
    {
        [OperationContract]
        void queryByBrand(string client, String brand);

        [OperationContract]
        void queryByMaxPrice(string client, int maxPrice);

        [OperationContract]
        void queryByMinimumYearRegistration(string client, int minYearRegistration);
    }
}
