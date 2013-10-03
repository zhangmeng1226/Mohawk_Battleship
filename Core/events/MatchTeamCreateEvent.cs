using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using MBC.Shared;

namespace MBC.Core.Events
{
    public class MatchTeamCreateEvent : Event
    {
        public MatchTeamCreateEvent(IDNumber teamID, string teamName)
        {
            TeamID = teamID;
            TeamName = teamName;
        }

        private MatchTeamCreateEvent(SerializationInfo info, StreamingContext context)
        {

        }

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        public IDNumber TeamID
        {
            get;
            private set;
        }

        public string TeamName
        {
            get;
            private set;
        }

        public override Type EventType
        {
            get
            {
                return Type.MatchTeamCreate;
            }
        }
    }
}
