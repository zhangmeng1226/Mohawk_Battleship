using MBC.Core.Matches;
using System;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Match"/> that has ended.
    /// </summary>
    public class MatchEndEvent : MatchEvent
    {
        protected internal override void GenerateMessage()
        {
            Message = "The match has ended.";
        }
    }
}