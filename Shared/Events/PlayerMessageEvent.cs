using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Created during a match when a controller outputs a string message.
    /// </summary>
    [Serializable]
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
        /// Gets the message string that was generated from the player.
        /// </summary>
        public string Message
        {
            get;
            private set;
        }
    }
}