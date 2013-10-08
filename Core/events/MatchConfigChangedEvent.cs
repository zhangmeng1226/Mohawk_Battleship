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

        protected MatchConfigChangedEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public MatchConfig Config
        {
            get;
            private set;
        }

        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}