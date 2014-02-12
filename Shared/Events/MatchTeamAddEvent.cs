using MBC.Shared;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Created during a match when a team has been created.
    /// </summary>
    public class MatchTeamAddEvent : Event
    {
        /// <summary>
        /// Constructs the event with the new team.
        /// </summary>
        /// <param name="team"></param>
        public MatchTeamAddEvent(Team team)
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
    }
}