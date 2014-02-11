using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MBC.Core.Events
{
    /// <summary>
    /// Created during a match when a controller outputs a string message.
    /// </summary>
    public class PlayerMessageEvent : PlayerEvent
    {
        /// <summary>
        /// Constructs this event with the player and message it created.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="message"></param>
        public PlayerMessageEvent(Player player, string message)
            : base(player)
        {
            Message = message;
        }

        /// <summary>
        /// Constructs this event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PlayerMessageEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the message string that was generated from the player.
        /// </summary>
        public string Message
        {
            get;
            private set;
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