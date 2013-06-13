using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MBC.Core.Accolades;

namespace MBC.Core.Events
{
    public class RoundAccoladeEvent : RoundEvent
    {
        public RoundAccoladeEvent(Round round, Accolade accolade) : base(round)
        {

        }
    }
}
