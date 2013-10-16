using System.Runtime.Serialization;
using MBC.Shared;

namespace MBC.Core.Events
{
    public class MatchConfigChangedEvent : Event
    {
        public MatchConfigChangedEvent(MatchConfig config)
        {
            Config = new MatchConfig(config);
        }

        public MatchConfigChangedEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public MatchConfig Config
        {
            get;
            private set;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}