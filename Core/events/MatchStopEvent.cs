namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Match"/> that has been stopped, but not ended.
    /// </summary>
    public class MatchStopEvent : MatchEvent
    {
        /// <summary>
        /// Passes the <paramref name="match"/> to the base constructor and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="match">The associated <see cref="Match"/>.</param>
        public MatchStopEvent(Match match)
            : base(match)
        {
            message = "The match has been stopped.";
        }
    }
}