using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Players
{
    public class NullPlayer
    {
        private string playerName;

        public NullPlayer(string name)
        {
            playerName = name;
        }

        public override string ToString()
        {
            return playerName;
        }
    }
}
