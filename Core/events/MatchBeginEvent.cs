using MBC.Core.Game;
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
        /// Deprecated. Constructs this event.
        /// </summary>
        /// <param name="matchID"></param>
        [Obsolete("IDNumber is not necessary.")]
        public MatchBeginEvent(IDNumber matchID)
        {
        }

        /// <summary>
        /// Constructs this event
        /// </summary>
        public MatchBeginEvent()
        {
        }

        /// <summary>
        /// Constructs this event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public MatchBeginEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Deprecated. Returns null
        /// </summary>
        [Obsolete("IDNumber is not necessary.")]
        public IDNumber MatchID
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the serialization data from the event.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}