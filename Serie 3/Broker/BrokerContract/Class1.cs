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

        [OperationContract]
        string helloWorld(string name);
        [OperationContract]
        void submitQueryByBrand(string client, string brand);

    }

    [DataContract]
    public class QueryStructure
    {

        [DataMember]
        string clientUri { get; set; }

        [DataMember]
        string query { get; set; }
    }
}
