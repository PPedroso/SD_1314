using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace BrokerStandContract
{

    [ServiceContract(Namespace = "http://ISEL.BROKER.STAND")]
    public interface IBrokerStand
    {
        [OperationContract]
        void registerStand(string standEndpoint);
    }
}
