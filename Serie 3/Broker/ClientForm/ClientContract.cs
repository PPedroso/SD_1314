using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ClientForm
{
    [ServiceContract(Namespace="http://ISEL.CLIENT")]
    public interface ClientContract
    {
        [OperationContract]
        void submitProposal(string stand, string proposal);
    }
}
