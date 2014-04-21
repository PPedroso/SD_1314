using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientJob
{
    public interface IJob
    {
        String getExeName();
        String getInputPath();
        String getOutputPath();
    }
}
