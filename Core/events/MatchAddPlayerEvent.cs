using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using MBC.Core.Matches;
using MBC.Core.Players;
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

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        public string PlayerName
        {
            get;
            private set;
        }

        public IDNumber PlayerID
        {
            get;
            private set;
        }
    }
}
