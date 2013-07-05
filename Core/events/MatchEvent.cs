using MBC.Core.Matches;

namespace MBC.Core.Events
{
    /// <summary>
    /// The base class for a series of <see cref="Event"/>s that are related to a <see cref="Match"/>.
    /// </summary>
    public abstract class MatchEvent : Event
    {
        /// <summary>
        /// Stores a <see cref="Match"/>.
        /// </summary>
        /// <param name="match">The <see cref="Match"/> to store.</param>
        public MatchEvent(Match match)
        {
            Match = match;
        }

        /// <summary>
        /// Gets the <see cref="Match"/> associated with this <see cref="Event"/>.
        /// </summary>
        public Match Match
        {
            get;
            private set;
        }
    }
}