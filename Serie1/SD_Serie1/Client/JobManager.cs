using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientJob;
using JobImplementation;

namespace Client
{
    class JobManager
    {
        Dictionary<Int32, IJob> dict = new Dictionary<Int32, IJob>();
        Int32 

        Job createJob(String exName,String inputFile,String outputFile){
            Job j = new Job(exName, inputFile, outputFile);


            return j; 
                
        }


        void sendJob(Job j) { 
            
        }
    }
}
