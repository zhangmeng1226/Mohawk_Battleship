using MBC.Core.Accolades;
using MBC.Core.Events;
using MBC.Core.Matches;
using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MBC.Core.Rounds
{

    public abstract class Round : MBCObject
    {

        public Round(Match match)
        {
            Parent = match;
        }
    }
}