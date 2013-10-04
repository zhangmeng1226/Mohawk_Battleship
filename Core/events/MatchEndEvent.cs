using System.Runtime.Serialization;
using MBC.Core.Matches;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Match"/> that has ended.
    /// </summary>
    public class MatchEndEvent : Event
    {
        public MatchEndEvent()
        {
        }

        private MatchEndEvent(SerializationInfo info, StreamingContext context)
        {
        }

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}