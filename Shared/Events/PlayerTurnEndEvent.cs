using MBC.Shared;
using MBC.Shared.Entities;
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
    public class PlayerTurnEndEvent : PlayerEvent
    {
        /// <summary>
        /// Constructs the event with the player that switched their turn.
        /// </summary>
        public PlayerTurnEndEvent(Player player)
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
            Player tmp = Player.Match.TurnOrder.First();
            Player.Match.TurnOrder[0] = Player.Match.TurnOrder[Player.Match.TurnOrder.Count - 1];
            Player.Match.TurnOrder[Player.Match.TurnOrder.Count - 1] = tmp;
            Player.Match.CurrentPlayer = Player.Match.TurnOrder.First();
        }
    }
}