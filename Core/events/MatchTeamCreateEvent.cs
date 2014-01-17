using MBC.Shared;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Created during a match when a team has been created.
    /// </summary>
    public class MatchTeamCreateEvent : Event
    {
        /// <summary>
        /// Constructs the event with the new team.
        /// </summary>
        /// <param name="team"></param>
        public MatchTeamCreateEvent(Team team)
        {
            Team = team;
        }

        /// <summary>
        /// Constructs the event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public MatchTeamCreateEvent(SerializationInfo info, StreamingContext context)
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