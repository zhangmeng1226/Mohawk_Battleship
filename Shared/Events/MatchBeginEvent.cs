using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Match"/> that has begun.
    /// </summary>
    public class MatchBeginEvent : Event
    {
        /// <summary>
        /// Constructs this event
        /// </summary>
        public MatchBeginEvent()
        {
        }
    }
}