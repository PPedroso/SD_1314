﻿using System;
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

using ClientForm.Broker;
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
        static string CLIENT_SERVICE_ENDPOINT;

        BrokerClientClient proxy;
        public static Form _Form1;
        static ServiceHost svcHost;

        private static void RunService() {
            svcHost = new ServiceHost(typeof(ClientService));
            svcHost.Open();
            CLIENT_SERVICE_ENDPOINT = svcHost.BaseAddresses[0].ToString();

        }

        protected override void OnFormClosed(FormClosedEventArgs e) {
            base.OnFormClosed(e);
            svcHost.Close();
        }


        public void updateTextBox(Proposal p){
            listBox1.BeginInvoke(new Action(() => listBox1.Items.Add(p)));
        }

        public Form1() {
            InitializeComponent();
            _Form1 = this;
            proxy = new BrokerClientClient();
            RunService();
        }

        private void button1_Click(object sender, EventArgs e) {
            listBox1.Items.Clear();
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
                listBox1.Items.Add("Failed to contact the necessary services");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Proposal p = (Proposal)listBox1.SelectedItem;
            EndpointAddress addr = new EndpointAddress(p.endpoint);
            BasicHttpBinding bind = new BasicHttpBinding();

            IStandClient proxy;
            IChannelFactory<IStandClient> cfact = new ChannelFactory<IStandClient>(bind);

            proxy = cfact.CreateChannel(addr);

            try { 
                proxy.reserveCar(p.id);
            } catch(FaultException<StandClientContract.AlreadyReservedFault> ex) {
                MessageBox.Show(String.Format("Car with id : {0} is already reserved", ex.Detail.Id));
            }
        }
    }
}
