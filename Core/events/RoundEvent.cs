using MBC.Core.Rounds;
using System;
using System.Xml.Serialization;
using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// The base class for a series of <see cref="Event"/>s that relate to changes in a <see cref="Round"/>.
    /// </summary>
    public abstract class RoundEvent : Event
    {
        protected RoundEvent()
        {

        }

        public RoundEvent(IDNumber roundID)
        {

        }

        public IDNumber RoundID
        {
            get;
            private set;
        }
    }
}