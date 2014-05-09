using IBrokerCAO;
using JobImplementation;
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace Client
{
    class Client
    {
        static readonly string SERVER_EP = "JobBrokering";

        static void Main()
        {
            Console.WriteLine("Testing");

            
            //Registo do canal
            TcpChannel ch = new TcpChannel(0);
            ChannelServices.RegisterChannel(ch, false);

            //Criação do proxy para o objecto do broker
            IMyBrokerCAO brokerProxy = (IMyBrokerCAO)Activator.GetObject(typeof(IMyBrokerCAO), "tcp://localhost:1234/" + SERVER_EP);

            //Submissão do trabalho ao broker
            Job j = new Job("service.exe", "inputFile1", "inputFile2", "clientEndPoint");
            brokerProxy.SubmitJob(j);


            //Console.WriteLine("Job has completed: " + brokerProxy.RequestJobStatus(j.getJobId()));            
        }
    }
}
