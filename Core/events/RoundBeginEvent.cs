using System.Runtime.Serialization;
using MBC.Core.Rounds;
using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="GameLogic"/> that has begun.
    /// </summary>
    public class RoundBeginEvent : RoundEvent
    {
        /// <summary>
        /// Passes the <paramref name="round"/> to the base constructor and generates a <see cref="Event.Message"/>
        /// based on the <see cref="MBC.Shared.Register"/>s that are involved in it.
        /// </summary>
        /// <param name="round">The associated <see cref="GameLogic"/>.</param>
        public RoundBeginEvent(IDNumber roundID)
            : base(roundID)
        {
            RoundID = roundID;
        }

        protected RoundBeginEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IDNumber RoundID
        {
            get;
            private set;
        }

        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}