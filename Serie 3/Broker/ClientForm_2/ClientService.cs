using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ClientContract;

namespace ClientForm
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ClientService:IClient
    {
        public void submitProposal(string endpoint,int id ,string brand,int year, int price)
        {

            Proposal p = new Proposal(id,brand,endpoint,year,price);
            ((Form1)Form1._Form1).updateTextBox(p);
        }
    }
}
