using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MBC.Shared.Events
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
            : base(nextPlayer)
        {
        }

        /// <summary>
        /// Gets the player who will have the next turn.
        /// </summary>
        public Player PrevPlayer
        {
            get;
            private set;
        }
    }
}