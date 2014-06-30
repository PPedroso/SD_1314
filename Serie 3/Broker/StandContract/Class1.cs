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
        void submitQuery(string query);

    }
}
