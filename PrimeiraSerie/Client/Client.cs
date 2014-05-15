using BrokerSAO;
using JobImplementation;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    class JobManager
    {
        private static JobManager jobManager = new JobManager();

        public static JobManager getInstance() {
            return jobManager;
        }

        private readonly Object lockObj = new object();
        private readonly Dictionary<long, Job> dict = new Dictionary<long, Job>();
        
        private JobManager() {
        }

        public void addJob(Job j) {
            lock (lockObj) {
                dict.Add(j.getJobId(), j);
            }
        }

        public Job removeJob(long id) {
            lock (lockObj) {
                Job j = dict[id];
                dict.Remove(id);
                return j;
            }
        }    
    }

    public class clientEndJob : MarshalByRefObject, IEndJob
    {
        private Action<Object> action;
        private TaskScheduler scheduler;

        public clientEndJob() {
            //nothing
        }

        public clientEndJob(Action<Object> action, TaskScheduler scheduler)
        {
            this.action = action;
            this.scheduler = scheduler;
        }

        public void finish(long jobId)
        {
            Console.WriteLine("Mandou fechar o job com id " + jobId);
            Job j = JobManager.getInstance().removeJob(jobId);
            if (action != null && scheduler != null) {
                Task.Factory.StartNew(action, j, CancellationToken.None, TaskCreationOptions.None, scheduler);
            }            
        }
    }

    class Client
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Client Started");

            string configFile = "Client.exe.config";

            RemotingConfiguration.Configure(configFile, false);
            WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
            IBrokerSAO brokerProxy = (IBrokerSAO)Activator.GetObject(entries[0].ObjectType, entries[0].ObjectUrl);

            //create jobManager
            JobManager jobManager = JobManager.getInstance();

            //Submissão do trabalho ao broker
            //int numberOfJobs = 1000;
            //for(int i=0;i<numberOfJobs;i++){
            //    Job j = new Job("orderByCrescent.exe", "inputFile"+i, "inputFile"+i+1, "clientEndPoint", new clientEndJob());
            //    long id = brokerProxy.SubmitJob(j);
            //    j.setJobId(id);
                
            //    jobManager.addJob(j);
            //}

            Job j1 = new Job("Sum.exe", "inputNumbers.txt", "inputNumbersSum.txt", new clientEndJob());
            long id = brokerProxy.SubmitJob(j1);
            j1.setJobId(id);
            jobManager.addJob(j1);

            Job j2 = new Job("OrderByCrescent.exe", "inputNumbers.txt", "inputNumbersOrder.txt", new clientEndJob());
            id = brokerProxy.SubmitJob(j2);
            j2.setJobId(id);
            jobManager.addJob(j2);
            Console.ReadLine();

            Console.WriteLine("Job" + j1.getJobId() + " status = " + brokerProxy.RequestJobStatus(j1.getJobId()));
            Console.WriteLine("Job" + j2.getJobId() + " status = " + brokerProxy.RequestJobStatus(j2.getJobId()));

            Console.ReadLine();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new ClientForm(brokerProxy));
        }
    }
}
