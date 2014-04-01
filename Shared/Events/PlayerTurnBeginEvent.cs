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
    }
}