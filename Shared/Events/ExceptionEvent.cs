using MBC.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class ExceptionEvent : Event
    {
        public ExceptionEvent(Entity exEntity, Exception exception)
            : base(exEntity)
        {
            Exception = exception;
        }

        public Exception Exception
        {
            get;
            private set;
        }
    }
}