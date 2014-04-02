using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/> that had taken too long to return from
    /// a method call in its associating <see cref="Controller"/>.
    /// </summary>
    [Serializable]
    public class PlayerTimeoutEvent : PlayerEvent
    {
        /// <summary>
        /// Constructs the event from the player and exception generated from the timeout.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="exception"></param>
        public PlayerTimeoutEvent(Player player, string method)
            : base(player)
        {
            Method = method;
        }

        /// <summary>
        /// Gets a string identifying the method call made to a <see cref="Controller"/> through the
        /// associated <see cref="PlayerEvent.Register"/>.
        /// </summary>
        public string Method
        {
            get;
            private set;
        }

        protected internal override void PerformOperation()
        {
            if (!Player.Active)
            {
                throw new InvalidEventException(this, "The player is inactive.");
            }
            Player.Timeouts++;
        }
    }
}