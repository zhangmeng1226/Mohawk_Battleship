using MBC.Core.Matches;
using System;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Match"/> that has begun.
    /// </summary>
    public class MatchBeginEvent : MatchEvent
    {
        protected internal override void GenerateMessage()
        {
            Message = "The match has begun.";
        }
    }
}