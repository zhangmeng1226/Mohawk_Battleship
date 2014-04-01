using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class PlayerTurnEndEvent : PlayerEvent
    {
        public PlayerTurnEndEvent(Player player)
            : base(player)
        {
        }
    }
}