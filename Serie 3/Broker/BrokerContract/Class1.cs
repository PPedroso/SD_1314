using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace BrokerClientContract
{

    [ServiceContract(Namespace = "http://ISEL.BROKER.CLIENT")]
    public interface IBrokerClientService
    {   
        [OperationContract(IsOneWay = true)]
        void submitQueryByBrand(string client, string brand);

        [OperationContract(IsOneWay = true)]
        void submitQueryByMinumumYearRegistration(string client, int yearRegistration);

        [OperationContract(IsOneWay = true)]
        void submitQueryByMaxPrice(string client, int maxPrice);
    }
}
