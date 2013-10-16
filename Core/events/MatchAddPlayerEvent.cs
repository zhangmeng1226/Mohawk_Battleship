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

        public MatchAddPlayerEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
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

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}