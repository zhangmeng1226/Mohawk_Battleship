using System.Runtime.Serialization;
using MBC.Shared;

namespace MBC.Core.Events
{
    public class MatchAddPlayerEvent : Event
    {
        public MatchAddPlayerEvent(IDNumber newPlayer, string plrName)
        {
            PlayerID = newPlayer;
            PlayerName = plrName;
        }

        private MatchAddPlayerEvent(SerializationInfo info, StreamingContext context)
        {
        }

        public IDNumber PlayerID
        {
            get;
            private set;
        }

        public string PlayerName
        {
            get;
            private set;
        }

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}