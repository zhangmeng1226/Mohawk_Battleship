using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core
{
    public class Register : ControllerRegister
    {
        public Register(Register copy) : base(copy)
        {
            if (copy != null)
            {
                Ships = new ShipList(copy.Ships);
                ShipsLeft = new ShipList(copy.ShipsLeft);
                Shots = new ShotList(copy.Shots);
                ShotsAgainst = new ShotList(copy.ShotsAgainst);
            }
        }

        public Register(MatchInfo match, ControllerID id)
            : base(match, id) { }

        private Register() { }


    }
}
