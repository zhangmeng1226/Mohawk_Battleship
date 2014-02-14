using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// The base class for a series of <see cref="Event"/>s that provide information on the actions
    /// made in rounds from controllers. Provides the <see cref="IDNumber"/> of the <see cref="Controller"/>
    /// that caused the event to be created.
    /// </summary>
    public abstract class PlayerEvent : Event
    {
        /// <summary>
        /// Constructs this event with the given player.
        /// </summary>
        /// <param name="player"></param>
        public PlayerEvent(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Gets the player associated with this event.
        /// </summary>
        public Player Player
        {
            get;
            private set;
        }
    }
}