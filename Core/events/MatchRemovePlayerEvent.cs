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

        public MatchRemovePlayerEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IDNumber PlayerID
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