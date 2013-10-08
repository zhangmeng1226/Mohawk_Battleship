using System.Runtime.Serialization;
using MBC.Core.Rounds;
using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// The base class for a series of <see cref="Event"/>s that relate to changes in a <see cref="GameLogic"/>.
    /// </summary>
    public abstract class RoundEvent : Event
    {
        public RoundEvent(IDNumber roundID)
        {
        }

        public RoundEvent(SerializationInfo info, StreamingContext context)
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