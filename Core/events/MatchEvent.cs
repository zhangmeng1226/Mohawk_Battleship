namespace MBC.Core.Events
{
    /// <summary>
    /// Defines a method that retrieves and handles a <see cref="MatchEvent"/>.
    /// </summary>
    /// <param name="ev">The generated <see cref="MatchEvent"/>.</param>
    /// <seealso cref="MatchEvent"/>
    public delegate void MBCMatchEventHandler(MatchEvent ev);

    /// <summary>
    /// The base class for a series of <see cref="Event"/>s that are related to a <see cref="Match"/>.
    /// </summary>
    public abstract class MatchEvent : Event
    {
        private Match match;

        /// <summary>
        /// Stores a <see cref="Match"/>.
        /// </summary>
        /// <param name="match">The <see cref="Match"/> to store.</param>
        public MatchEvent(Match match)
        {
            this.match = match;
        }

        /// <summary>
        /// Gets the <see cref="Match"/> associated with this <see cref="Event"/>.
        /// </summary>
        public Match Match
        {
            get
            {
                return match;
            }
        }
    }
}