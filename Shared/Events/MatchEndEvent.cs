using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Match"/> that has ended.
    /// </summary>
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