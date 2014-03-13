using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="Match"/> that has ended.
    /// </summary>
    [Serializable]
    public class MatchEndEvent : Event
    {
        /// <summary>
        /// Constructs the event.
        /// </summary>
        public MatchEndEvent()
        {
        }
    }
}