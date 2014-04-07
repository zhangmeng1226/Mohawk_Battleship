using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Attributes
{
    /// <summary>
    /// Used to indicate that a certain MBCEventHandler handles a specific type of event.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class EventFilterAttribute : Attribute
    {
        /// <summary>
        /// Stores the given type of event.
        /// </summary>
        /// <param name="eventType"></param>
        public EventFilterAttribute(Type eventType)
        {
            Event = eventType;
        }

        /// <summary>
        /// Gets the type of event that the method handles.
        /// </summary>
        public Type Event
        {
            get;
            private set;
        }
    }
}