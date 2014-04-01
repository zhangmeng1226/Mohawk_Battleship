using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventFilterAttribute : Attribute
    {
        public EventFilterAttribute(Type eventType)
        {
            Event = eventType;
        }

        public Type Event
        {
            get;
            private set;
        }
    }
}