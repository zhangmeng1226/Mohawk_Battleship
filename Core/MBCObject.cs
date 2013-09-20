using System.Collections.Generic;
using System.Xml.Serialization;
using MBC.Core.Events;

namespace MBC.Core
{
    public abstract class MBCObject
    {
        private List<Event> events;

        public MBCObject()
        {
            events = new List<Event>();
        }

        public event MBCEventHandler EventCreated;

        [XmlIgnore]
        public MBCObject Parent
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Event"/>s that have been generated.
        /// </summary>
        private IList<Event> Events
        {
            get
            {
                return events.AsReadOnly();
            }
        }

        internal virtual void AttachEvent(Event ev)
        {
            events.Add(ev);
            if (EventCreated != null)
            {
                EventCreated(ev);
            }
        }
    }
}