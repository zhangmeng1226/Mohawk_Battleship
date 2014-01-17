using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// The base class for a series of <see cref="Event"/>s that provide information on the actions
    /// made in rounds from controllers. Provides the <see cref="IDNumber"/> of the <see cref="Controller"/>
    /// that caused the event to be created.
    /// </summary>
    public abstract class PlayerEvent : Event
    {
        /// <summary>
        /// Do not use.
        /// </summary>
        /// <param name="plrID"></param>
        [Obsolete("Old Framework")]
        public PlayerEvent(IDNumber plrID)
        {
            PlayerObj = new Player(plrID, "placeholder");
        }

        /// <summary>
        /// Constructs this event with the given player.
        /// </summary>
        /// <param name="player"></param>
        public PlayerEvent(Player player)
        {
            PlayerObj = player;
        }

        /// <summary>
        /// Constructs this event from serialization data
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PlayerEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Deprecated. Gets the ID number of the player in this event.
        /// </summary>
        [Obsolete("Get the player object and get the ID from there instead.")]
        public IDNumber Player
        {
            get
            {
                return PlayerObj.ID;
            }
        }

        /// <summary>
        /// Gets the player associated with this event.
        /// </summary>
        public Player PlayerObj
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the serialization data from this event.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}