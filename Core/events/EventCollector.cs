using System.Collections.Generic;
using System.Runtime.Serialization;

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

        public void AddEventGenerator(IEventGenerator gen)
        {
            gen.EventGenerated += EventGenerated;
        }

        protected virtual void EventGenerated(Event ev)
        {
            events.Add(ev);
        }

        protected void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //Serialize
        }

        private IEnumerator<Event> GetEnumerator()
        {
            return events.GetEnumerator();
        }
    }
}