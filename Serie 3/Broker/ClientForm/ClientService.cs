using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ClientForm
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ClientService:ClientContract
    {
        public void submitProposal(string stand,string proposal)
        {
            ((Form1)Form1._Form1).updateTextBox(proposal + " from " + stand );
        }
    }
}
