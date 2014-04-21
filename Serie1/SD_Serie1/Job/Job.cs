using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientJob;

namespace JobImplementation
{
    [Serializable]
    public class Job : IJob
    {
        String exeName;
        String inputPath;
        String outputPath;

        public Job(String e, String i, String o)
        {
            exeName = e;
            inputPath = i;
            outputPath = o;
        }

        public String getExeName() { return exeName; }
        public String getInputPath() { return inputPath; }
        public String getOutputPath() { return outputPath; }
    }
}
