using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    public class StateSaveEvent : Event
    {
        public StateSaveEvent()
        {
        }

        public StateSaveEvent(SerializationInfo info, StreamingContext context)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}