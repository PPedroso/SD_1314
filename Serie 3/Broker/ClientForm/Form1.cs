using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

using BrokerClientContract;
using StandContract;
using StandClientContract;

namespace ClientForm
{
    
    
    public partial class Form1 : Form
    {

        const string CLIENT_SERVICE_ENDPOINT = "http://localhost:8010/ClientService";
        EndpointAddress addr = new EndpointAddress("http://localhost:8080/BrokerClientService");
        BasicHttpBinding bind = new BasicHttpBinding();
        BrokerClientContract.IBrokerClientService proxy;
        public static Form _Form1;
        static ServiceHost svcHost;


        private static void RunService()
        {
            Uri addr = new Uri(CLIENT_SERVICE_ENDPOINT);
            BasicHttpBinding bind = new BasicHttpBinding();

            svcHost = new ServiceHost(typeof(ClientService));

            ServiceMetadataBehavior smb = svcHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (smb != null)
            {
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = addr;

            }
            else
            {
                smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = addr;
                svcHost.Description.Behaviors.Add(smb);
            }

            svcHost.AddServiceEndpoint(typeof(ClientContract), bind, addr);
            svcHost.Open();

        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            svcHost.Close();
        }


        public void updateTextBox(string str){
            listBox1.Items.Add(str);
        }

        public Form1()
        {
            InitializeComponent();
            _Form1 = this;
            IChannelFactory<IBrokerClientService> cfact = new ChannelFactory<IBrokerClientService>(bind);
            proxy = cfact.CreateChannel(addr);
            RunService();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                proxy.submitQueryByBrand("", textBox3.Text);
            }
            catch (Exception ex)
            {
                String str = ex.Message;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            EndpointAddress addr = new EndpointAddress("http://localhost:8010/ClientService");
            BasicHttpBinding bind = new BasicHttpBinding();

            StandClientContract.IStandClientContract proxy;

            IChannelFactory<StandClientContract.IStandClientContract> cfact = new ChannelFactory<StandClientContract.IStandClientContract>(bind);
            proxy = cfact.CreateChannel(addr);

            proxy.reserveCar(1);
        }
    }
}
