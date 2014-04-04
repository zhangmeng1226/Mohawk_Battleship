using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class PlayerTurnBeginEvent : PlayerEvent
    {
        public PlayerTurnBeginEvent(Player player)
            : base(player)
        {
        }

        protected internal override void PerformOperation()
        {
            if (!Player.Active)
            {
                throw new InvalidEventException(this, "The player is inactive.");
            }
            Player.Match.CurrentPlayer = Player.Match.TurnOrder.First();
        }
    }
}