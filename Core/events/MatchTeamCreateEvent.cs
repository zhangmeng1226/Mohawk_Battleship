using System.Runtime.Serialization;
using MBC.Shared;

namespace MBC.Core.Events
{
    public class MatchTeamCreateEvent : Event
    {
        private Team team;

        public MatchTeamCreateEvent(Team team)
        {
            this.team = team;
        }

        public MatchTeamCreateEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public Team Team
        {
            get
            {
                return new Team(team);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}