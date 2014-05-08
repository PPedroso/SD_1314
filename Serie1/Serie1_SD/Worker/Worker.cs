using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker
{
    class Worker
    {
        //arg0 - channel type
        //arg1 - uri
        static void Main(string[] args) {
            if (args.Length != 2)
            {
                throw new ArgumentException("Wrong number of arguments were given");
            }

        }
    }
}
