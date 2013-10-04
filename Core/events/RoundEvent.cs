using MBC.Core.Rounds;
using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// The base class for a series of <see cref="Event"/>s that relate to changes in a <see cref="Round"/>.
    /// </summary>
    public abstract class RoundEvent : Event
    {
        public RoundEvent(IDNumber roundID)
        {
        }

        protected RoundEvent()
        {
        }

        public IDNumber RoundID
        {
            get;
            private set;
        }
    }
}