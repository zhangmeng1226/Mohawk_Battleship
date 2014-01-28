using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MBC.Core.Events
{
    /// <summary>
    /// Created during a match when a player's turn has ended and is being switched to another player.
    /// </summary>
    public class PlayerTurnSwitchEvent : PlayerEvent
    {
        /// <summary>
        /// Constructs the event from the previous player and the next player.
        /// </summary>
        /// <param name="prevPlayer"></param>
        /// <param name="nextPlayer"></param>
        public PlayerTurnSwitchEvent(Player prevPlayer, Player nextPlayer)
            : base(prevPlayer)
        {
        }

        /// <summary>
        /// Constructs the event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PlayerTurnSwitchEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the player who will have the next turn.
        /// </summary>
        public Player NextPlayer
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