using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class InvalidEventException : Exception
    {
        public readonly Event InvalidEvent;

        public InvalidEventException(Event ev, string message)
            : base(message)
        {
            InvalidEvent = ev;
        }
    }
}