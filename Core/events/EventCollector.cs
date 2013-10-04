using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MBC.Core.Events
{
    public abstract class EventCollector : ISerializable, IEnumerable<Event>
    {
        private int currentEventIdx;
        private List<Event> events;

        public EventCollector()
        {
            events = new List<Event>();
            currentEventIdx = 0;
        }

        protected EventCollector(SerializationInfo info, StreamingContext context)
        {
            //Deserialize
        }

        protected void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //Serialize
        }

        public void AddEventGenerator(IEventGenerator gen)
        {
            gen.EventGenerated += EventGenerated;
        }

        protected virtual void EventGenerated(Event ev)
        {
            events.Add(ev);
        }

        IEnumerator<Event> GetEnumerator()
        {
            return events.GetEnumerator();
        }
    }
}
