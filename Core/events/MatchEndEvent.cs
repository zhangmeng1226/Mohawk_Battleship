using MBC.Core.Matches;
using System;
using System.Runtime.Serialization;

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