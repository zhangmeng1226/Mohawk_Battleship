using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MBC.Core.Events
{
    /// <summary>
    /// Created during a match when a team has been created.
    /// </summary>
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
        /// Constructs the event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public MatchTeamRemoveEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the created team
        /// </summary>
        public Team Team
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets serialization data from the event.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}