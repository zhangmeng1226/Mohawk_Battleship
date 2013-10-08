using System.Runtime.Serialization;
using MBC.Core.Rounds;
using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="GameLogic"/> that has ended.
    /// </summary>
    public class RoundEndEvent : RoundEvent
    {
        /// <summary>
        /// Passes the <paramref name="round"/> to the base constructor and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="round"></param>
        public RoundEndEvent(IDNumber roundID)
            : base(roundID)
        {
            RoundID = roundID;
        }

        public RoundEndEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IDNumber RoundID
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