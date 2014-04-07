using MBC.Shared;
using MBC.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Created during a match when a team has been created.
    /// </summary>
    [Serializable]
    public class MatchTeamRemoveEvent : MatchEvent
    {
        /// <summary>
        /// Constructs the event with the team to be removed.
        /// </summary>
        /// <param name="team"></param>
        public MatchTeamRemoveEvent(Match match, Team team)
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
            if (!Match.Teams.Remove(Team))
            {
                throw new InvalidEventException(this, "Team does not existin within the match.");
            }
        }
    }
}