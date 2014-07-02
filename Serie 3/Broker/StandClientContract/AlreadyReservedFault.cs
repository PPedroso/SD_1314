using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace StandClientContract
{
    [DataContract]
    public class AlreadyReservedFault {

        private int id;

        [DataMember]
        public int Id {
            get { return id; }
            set { id = value; }
        }

    }
}
