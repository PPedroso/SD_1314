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
using BrokerClientContract;

namespace ClientForm
{
    public partial class Form1 : Form
    {

        EndpointAddress addr = new EndpointAddress("http://localhost:8080/BrokerClientService");
        BasicHttpBinding bind = new BasicHttpBinding();
        BrokerClientContract.IBrokerClientService proxy;

        public Form1()
        {
            InitializeComponent();

            IChannelFactory<IBrokerClientService> cfact = new ChannelFactory<IBrokerClientService>(bind);
            proxy = cfact.CreateChannel(addr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            proxy.submitQueryByBrand(textBox1.Text, textBox3.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = proxy.helloWorld(textBox4.Text);
        }
    }
}
