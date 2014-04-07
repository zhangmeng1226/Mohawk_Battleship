using MBC.Shared.Entities;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="Match"/> that has ended.
    /// </summary>
    [Serializable]
    public class MatchEndEvent : MatchEvent
    {
        /// <summary>
        /// Constructs the event.
        /// </summary>
        public MatchEndEvent(Match match)
            : base(match)
        {
        }
    }
}