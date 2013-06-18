namespace MBC.Core.Events
{
    /// <summary>
    /// Defines a method that retrieves and handles a <see cref="RoundEvent"/>.
    /// </summary>
    /// <param name="ev">The generated <see cref="RoundEvent"/></param>
    public delegate void MBCRoundEventHandler(RoundEvent ev);

    /// <summary>
    /// The base class for a series of <see cref="Event"/>s that relate to changes in a <see cref="Round"/>.
    /// </summary>
    public abstract class RoundEvent : Event
    {
        private Round round;

        /// <summary>
        /// Stores the <paramref name="round"/>.
        /// </summary>
        /// <param name="round">The <see cref="Round"/> to store.</param>
        public RoundEvent(Round round)
        {
            this.round = round;
        }

        /// <summary>
        /// Gets the <see cref="Round"/> associated with this <see cref="Event"/>.
        /// </summary>
        public Round Round
        {
            get
            {
                return round;
            }
        }
    }
}