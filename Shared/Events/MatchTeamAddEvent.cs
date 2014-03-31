using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Created during a match when a team has been created.
    /// </summary>
    [Serializable]
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

        protected internal override void PerformOperation()
        {
            if (!Match.Teams.Add(Team))
            {
                throw new InvalidEventException(this, "Team already exists within match.");
            }
        }
    }
}