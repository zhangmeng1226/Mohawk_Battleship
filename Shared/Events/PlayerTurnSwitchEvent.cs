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
    [Serializable]
    public class PlayerTurnSwitchEvent : PlayerEvent
    {
        /// <summary>
        /// Constructs the event from the previous player and the next player.
        /// </summary>
        /// <param name="prevPlayer"></param>
        /// <param name="nextPlayer"></param>
        public PlayerTurnSwitchEvent(Player player)
            : base(player)
        {
        }

        protected internal override void PerformOperation()
        {
            if (!Player.Active)
            {
                throw new InvalidEventException(this, "The player is inactive.");
            }
            if (Player.Match.CurrentPlayer != Player)
            {
                throw new InvalidEventException(this, String.Format("Player {0} does not have the turn.", Player));
            }
            Player.Match.TurnOrder.Remove(Player);
            Player.Match.TurnOrder.Add(Player);
            Player.Match.CurrentPlayer = Player.Match.TurnOrder.First();
        }
    }
}