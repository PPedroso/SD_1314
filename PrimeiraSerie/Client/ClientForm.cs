using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using JobImplementation;
using BrokerSAO;

namespace Client
{
    public partial class ClientForm : Form
    {
        private IBrokerSAO brokerSAO;

        public ClientForm(IBrokerSAO brokerSAO)
        {
            this.brokerSAO = brokerSAO;
            InitializeComponent();
        }

        private void addJobButton_Click(object sender, EventArgs e) {
            String inputPath = inputPathTextBox.Text;
            String outputPath = outputPathTextBox.Text;
            String execName = executableTextBox.Text;

            if (String.IsNullOrWhiteSpace(inputPath) || String.IsNullOrWhiteSpace(outputPath) || String.IsNullOrWhiteSpace(execName)) {
                return;
            }
            
            Action<Object> endTask = (o) => {
                Job j = (Job)o;
                logTextBox.Text += String.Format("Finished job with id: {0}\n", j.getJobId());    
            };
            TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Job addedJob = null;
            Task.Factory.StartNew(() => {
                Console.WriteLine("começou");
                addedJob = new Job(execName, inputPath, outputPath, new clientEndJob(endTask, scheduler));
                return brokerSAO.SubmitJob(addedJob);
            }).ContinueWith((t) => {
                addedJob.setJobId(t.Result);
                JobManager.getInstance().addJob(addedJob);
                logTextBox.Text += String.Format("Started job with id: {0}\n", t.Result);    
            }, CancellationToken.None, TaskContinuationOptions.None, scheduler);
        }

        private void findStatusButton_Click(object sender, EventArgs e) {
            String idText = idStatusTextBox.Text;
            long id;
            if (String.IsNullOrWhiteSpace(idText) || !long.TryParse(idText, out id)) {
                return;
            }
            Task.Factory.StartNew(() => {
                
            }).ContinueWith((t) => {
                statusTextBox.Text += String.Format("Status for job with id {0} is: {1}\n", id, brokerSAO.RequestJobStatus(Convert.ToInt64(id)));
            }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
