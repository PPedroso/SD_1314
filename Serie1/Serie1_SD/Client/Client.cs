using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Http;
using JobImplementation;
using IBrokerCAO;

namespace Client
{
    class Client
    {
        static readonly string SERVER_EP = "JobBrokering.soap";

        static void Main()
        {
            Console.WriteLine("Testing");

            HttpChannel ch = new HttpChannel(0);
            ChannelServices.RegisterChannel(ch, false);

            //Registo do objecto remoto
            RemotingConfiguration.RegisterActivatedClientType(typeof(IMyBrokerCAO),
                                                              "http://localhost:1234/");

            //Criação do objecto remoto no broker
            IMyBrokerCAO mb = (IMyBrokerCAO)Activator.GetObject(typeof(IMyBrokerCAO),
                                                              "http://localhost:1234/" + SERVER_EP);


            //Submissão do trabalho ao broker
            Job j = new Job("service.exe", "inputFile1", "inputFile2", "clientEndPoint");
            mb.SubmitJob(j);
        }
    }
}
