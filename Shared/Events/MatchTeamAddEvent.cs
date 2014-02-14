using MBC.Shared;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Created during a match when a team has been created.
    /// </summary>
    public class MatchTeamAddEvent : MatchEvent
    {
        /// <summary>
        /// Constructs the event with the new team.
        /// </summary>
        /// <param name="team"></param>
        public MatchTeamAddEvent(Match match, Team team)
            : base(match)
        {
            Team = team;
        }

        /// <summary>
        /// Gets the created team
        /// </summary>
        public Team Team
        {
            get;
            private set;
        }

        public override bool ApplyBackward()
        {
            return Match.Teams.Remove(Team);
        }

        public override bool ApplyForward()
        {
            return Match.Teams.Add(Team);
        }
    }
}