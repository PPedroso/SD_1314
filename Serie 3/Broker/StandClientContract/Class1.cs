using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace StandClientContract
{
    [ServiceContract(Namespace = "http://ISEL.STAND.CLIENT")]
    public interface IStandClientContract
    {
        [OperationContract]
        [FaultContract(typeof(AlreadyReservedException))]
        void reserveCar(int id);
    }
}