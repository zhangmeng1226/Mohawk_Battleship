namespace MBC.Core.Events
{
    public class MatchStopEvent : MatchEvent
    {
        public MatchStopEvent(Match match)
            : base(match)
        {
            message = "The match has been stopped.";
        }
    }
}