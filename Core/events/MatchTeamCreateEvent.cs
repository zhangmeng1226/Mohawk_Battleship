using System.Runtime.Serialization;
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

        protected MatchTeamCreateEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
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

        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}