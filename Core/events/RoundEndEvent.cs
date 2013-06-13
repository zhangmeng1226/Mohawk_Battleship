using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class RoundEndEvent : RoundEvent
    {
        public RoundEndEvent(Round round)
            : base(round)
        {
            message = "This round has ended.";
        }
    }
}
