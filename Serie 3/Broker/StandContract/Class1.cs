using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace StandContract
{
    [ServiceContract(Namespace = "http://ISEL.STAND")]
    public interface IStand
    {
        [OperationContract]
        void queryByBrand(String brand);

        [OperationContract]
        void queryByMaxPrice(int maxPrice);

        [OperationContract]
        void queryByMinimumYearRegistration(int minYearRegistration);
    }
}
