using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared
{
    public class RegisterInfo
    {
        public RegisterInfo()
        {

        }

        public RegisterInfo(RegisterInfo copy)
        {
            Score = copy.Score;
            IsAlive = copy.IsAlive;
        }

        public int Score
        {
            get;
            set;
        }

        public bool IsAlive
        {
            get;
            set;
        }
    }
}
