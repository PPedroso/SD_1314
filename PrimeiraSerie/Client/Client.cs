using BrokerSAO;
using JobImplementation;
using System;
using System.Runtime.Remoting;

namespace Client
{
    class Client
    {
        class clientEndJob : MarshalByRefObject, IEndJob
        {
            public void finish(long jobId)
            {
                Console.WriteLine("Mandou fechar o job com id " + jobId);
            }
        }

        static void Main()
        {
            Console.WriteLine("Client Started");

            string configFile = "Client.exe.config";
            RemotingConfiguration.Configure(configFile, false);
            WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
            IBrokerSAO brokerProxy = (IBrokerSAO)Activator.GetObject(entries[0].ObjectType, entries[0].ObjectUrl);

            //Submissão do trabalho ao broker
            Job j = new Job("service.exe", "inputFile1", "inputFile2", "clientEndPoint", new clientEndJob());
            long id = brokerProxy.SubmitJob(j);
            
            Console.ReadLine();
            //Registo do canal
            //TcpChannel ch = new TcpChannel(0);
            //ChannelServices.RegisterChannel(ch, false);

            ////Criação do proxy para o objecto do broker
            //IMyBrokerCAO brokerProxy = (IMyBrokerCAO)Activator.GetObject(typeof(IMyBrokerCAO), "tcp://localhost:1234/" + SERVER_EP);

            
            //Console.WriteLine("Job has completed: " + brokerProxy.RequestJobStatus(j.getJobId()));            
        }
    }
}
