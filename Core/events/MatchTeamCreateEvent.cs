﻿using System.Runtime.Serialization;
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

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}