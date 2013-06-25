namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Match"/> that has been started in a different thread.
    /// </summary>
    public class MatchPlayEvent : MatchEvent
    {
        /// <summary>
        /// Passes the <paramref name="match"/> to the base constructor and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="match">The associated <see cref="Match"/>.</param>
        public MatchPlayEvent(Match match)
            : base(match)
        {
            message = "The match is now running.";
        }
    }
}