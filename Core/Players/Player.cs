using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.Shared;

namespace MBC.Core.Players
{
    public class Player
    {
        public Player(IDNumber id, string name)
        {
            Register = new Register(id, name);
        }

        public Player(Player copy)
        {
            Register = new Register(copy.Register);
            Team = new Team(copy.Team);
        }

        public virtual Register Register { get; set; }

        public virtual Team Team { get; set; }
    }
}
