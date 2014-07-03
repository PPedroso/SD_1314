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
using System.Runtime.Serialization;

using BrokerClientContract;
using StandContract;
using StandClientContract;
using ClientContract;

namespace ClientForm
{
    
    
    public partial class Form1 : Form
    {
        const string QUERY_BY_BRAND = "Brand";
        const string QUERY_BY_PRICE = "Price";
        const string QUERY_BY_YEAR = "Year of Registration";
        const string CLIENT_SERVICE_ENDPOINT = "http://localhost:8010/ClientService";
        EndpointAddress addr = new EndpointAddress("http://localhost:8080/BrokerClientService");
        BasicHttpBinding bind = new BasicHttpBinding();
        BrokerClientContract.IBrokerClientService proxy;
        public static Form _Form1;
        static ServiceHost svcHost;

        private static void RunService() {
            Uri addr = new Uri(CLIENT_SERVICE_ENDPOINT);
            BasicHttpBinding bind = new BasicHttpBinding();
            svcHost = new ServiceHost(typeof(ClientService));

            ServiceMetadataBehavior smb = svcHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (smb != null) {
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = addr;

            } else {
                smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = addr;
                svcHost.Description.Behaviors.Add(smb);
            }

            svcHost.AddServiceEndpoint(typeof(IClient), bind, addr);
            svcHost.Open();

        }

        protected override void OnFormClosed(FormClosedEventArgs e) {
            base.OnFormClosed(e);
            svcHost.Close();
        }


        public void updateTextBox(Proposal p){
            listBox1.Items.Add(p);
        }

        public Form1() {
            InitializeComponent();
            _Form1 = this;
            IChannelFactory<IBrokerClientService> cfact = new ChannelFactory<IBrokerClientService>(bind);
            proxy = cfact.CreateChannel(addr);
            RunService();
        }

        private void button1_Click(object sender, EventArgs e) {
            listBox1.Text = "";
            try {
                int n;
                string queryType = comboBox1.Text;
                switch (queryType) {
                    case QUERY_BY_BRAND:
                        proxy.submitQueryByBrand(CLIENT_SERVICE_ENDPOINT, textBox3.Text);
                        break;
                    case QUERY_BY_PRICE:
                        if (int.TryParse(textBox3.Text, out n)) {
                            proxy.submitQueryByMaxPrice(CLIENT_SERVICE_ENDPOINT, n);
                        }
                        break;
                    case QUERY_BY_YEAR:
                        if (int.TryParse(textBox3.Text, out n)) {
                            proxy.submitQueryByMinumumYearRegistration(CLIENT_SERVICE_ENDPOINT, n);
                        }
                        break;
                }
            } catch (Exception) {
                listBox1.Text = "Failed to contact the necessary services";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Proposal p = (Proposal)listBox1.SelectedItem;
            var a = p.endpoint;
            EndpointAddress addr = new EndpointAddress(p.endpoint);
            BasicHttpBinding bind = new BasicHttpBinding();

            IStandClientContract proxy;
            IChannelFactory<IStandClientContract> cfact = new ChannelFactory<IStandClientContract>(bind);

            proxy = cfact.CreateChannel(addr);

            try { 
                proxy.reserveCar(p.id);
            } catch(FaultException<StandClientContract.AlreadyReservedFault> ex) {
                MessageBox.Show(String.Format("Car with id : {} is already reserved", ex.Detail.Id));
            }
        }
    }
}
