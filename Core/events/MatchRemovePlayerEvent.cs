using System.Runtime.Serialization;
using MBC.Shared;

namespace MBC.Core.Events
{
    public class MatchRemovePlayerEvent : Event
    {
        public MatchRemovePlayerEvent(IDNumber newPlayer)
        {
            PlayerID = newPlayer;
        }

        protected MatchRemovePlayerEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IDNumber PlayerID
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