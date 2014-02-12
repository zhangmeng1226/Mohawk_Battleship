using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Event that is created when the match parameters have changed.
    /// </summary>
    public class MatchConfigChangedEvent : Event
    {
        /// <summary>
        /// Constructs this event.
        /// </summary>
        public MatchConfigChangedEvent()
        {
        }
    }
}