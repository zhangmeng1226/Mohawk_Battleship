using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.Shared;

namespace MBC.Core.Players
{
    public class Player : IPlayer
    {
        public Player(IPlayer copy)
        {
            Register = new Register(copy.Register);
            Field = new FieldInfo(copy.Field);
            Match = new MatchConfig(copy.Match);
            Team = new Team(copy.Team);
        }

        public Register Register { get; set; }

        public FieldInfo Field { get; set; }

        public MatchConfig Match { get; set; }

        public Team Team { get; set; }
    }
}
