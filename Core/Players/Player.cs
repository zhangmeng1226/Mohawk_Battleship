using MBC.Core.Util;
using MBC.Shared;
using System;
using System.Diagnostics;
using System.Threading;
using MBC.Core.Events;
using System.Collections.Generic;
using MBC.Core.Matches;

namespace MBC.Core
{

    public abstract class Player : MBCObject
    {
        public ShipList Ships { get; internal set; }
        public ShotList Shots { get; internal set; }
        public ShipList ShipsLeft { get; internal set; }
        public ShotList ShotsAgainst { get; internal set; }
        public Register Register { get; internal set; }
    }
}