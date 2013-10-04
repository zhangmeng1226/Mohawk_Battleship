using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using MBC.Core.Matches;
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

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        public IDNumber PlayerID
        {
            get;
            private set;
        }
    }
}
