using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StandClientContract
{
    public class AlreadyReservedException : Exception
    {
        public AlreadyReservedException() { }

        public AlreadyReservedException(String message) : base(message) { }

        public AlreadyReservedException(String message, Exception cause) : base(message, cause) { }
    }
}
