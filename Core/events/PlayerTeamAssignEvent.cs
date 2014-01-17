using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Created during a match when a player has been assigned to a team.
    /// </summary>
    public class PlayerTeamAssignEvent : PlayerEvent
    {
        /// <summary>
        /// Constructs the event with the player id and the team id that they were assigned to.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="teamAssigned"></param>
        [Obsolete("Old framework")]
        public PlayerTeamAssignEvent(IDNumber player, IDNumber teamAssigned)
            : base(player)
        {
            Team = new Team(teamAssigned, "placeholder");
        }

        /// <summary>
        /// Constructs the event with the player and the team that they were assigned to.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="teamAssigned"></param>
        public PlayerTeamAssignEvent(Player player, Team teamAssigned)
            : base(player)
        {
            Team = teamAssigned;
        }

        /// <summary>
        /// Constructs the event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PlayerTeamAssignEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Deprecated. Gets the player ID that had been reassigned.
        /// </summary>
        [Obsolete("Use the player object directly.")]
        public IDNumber PlayerID
        {
            get
            {
                return PlayerObj.ID;
            }
        }

        /// <summary>
        /// Gets the team that the player has been assigned to.
        /// </summary>
        public Team Team
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ID number of the team that the player had been assigned to.
        /// </summary>
        [Obsolete("Use the team object directly.")]
        public IDNumber TeamID
        {
            get
            {
                return Team.ID;
            }
        }

        /// <summary>
        /// Gets the serialization data from the event.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}