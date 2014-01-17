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
        /// Deprecated. Constructs this event with the player ID and message it created.
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="message"></param>
        [Obsolete("Old framework")]
        public PlayerMessageEvent(IDNumber playerID, string message)
            : base(playerID)
        {
            Message = message;
        }

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
        /// Deprecated. Gets the player ID that sent the message.
        /// </summary>
        [Obsolete("Use the player object directly instead.")]
        public IDNumber PlayerID
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