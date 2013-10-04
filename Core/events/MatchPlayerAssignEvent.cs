using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using MBC.Core.Matches;
using MBC.Shared;

namespace MBC.Core.Events
{
    public class MatchPlayerAssignEvent : Event
    {
        public MatchPlayerAssignEvent(IDNumber player, IDNumber teamAssigned)
        {
            PlayerID = player;
            TeamID = teamAssigned;
        }

        public MatchPlayerAssignEvent(SerializationInfo info, StreamingContext context)
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        public IDNumber PlayerID
        {
            get;
            private set;
        }

        public IDNumber TeamID
        {
            get;
            private set;
        }
    }
}
