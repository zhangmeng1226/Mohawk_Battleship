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

        private MatchRemovePlayerEvent(SerializationInfo info, StreamingContext context)
        {
        }

        public IDNumber PlayerID
        {
            get;
            private set;
        }

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}