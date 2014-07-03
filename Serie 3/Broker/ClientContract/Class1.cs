using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ClientContract
{
    [ServiceContract(Namespace = "http://ISEL.CLIENT")]
    public interface IClient
    {
        [OperationContract]
        void submitProposal(string endpoint, int id, string brand, int year, int price);
    }
}
