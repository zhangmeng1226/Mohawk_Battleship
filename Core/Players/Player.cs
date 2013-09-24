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

    public class Player
    {
        /// <summary>
        /// Gets or sets the <see cref="Register"/> that is available for manipulation.
        /// </summary>
        public Register Register { get; internal set; }

        public FieldInfo Field { get; internal set; }

        public RegisterInfo Stats { get; internal set; }

        public MatchInfo Match { get; internal set; }

        public Team Team { get; internal set; }
    }
}