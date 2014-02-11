using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/> that had taken too long to return from
    /// a method call in its associating <see cref="Controller"/>.
    /// </summary>
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
        /// Constructs the event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PlayerTimeoutEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
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

        /// <summary>
        /// Gets the serialization data from the event
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}