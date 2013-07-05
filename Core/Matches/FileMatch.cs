using MBC.Core.Rounds;
using MBC.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Matches
{
    public class FileMatch : Match
    {

        public FileMatch(string fileName)
            : base(Configuration.Global)
        {

        }

        internal override Round CreateNewRound()
        {
            throw new NotImplementedException();
        }
    }
}
