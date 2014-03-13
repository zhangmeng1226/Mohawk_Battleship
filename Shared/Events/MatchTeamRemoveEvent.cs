using MBC.Shared;
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
    public class MatchTeamRemoveEvent : Event
    {
        /// <summary>
        /// Constructs the event with the team to be removed.
        /// </summary>
        /// <param name="team"></param>
        public MatchTeamRemoveEvent(Team team)
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