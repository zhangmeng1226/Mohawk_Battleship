using System.Runtime.Serialization;
using MBC.Core.Matches;
using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Match"/> that has begun.
    /// </summary>
    public class MatchBeginEvent : Event
    {
        public MatchBeginEvent(IDNumber matchID)
        {
            MatchID = matchID;
        }

        public MatchBeginEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IDNumber MatchID
        {
            get;
            private set;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}