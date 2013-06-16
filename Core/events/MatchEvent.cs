namespace MBC.Core.Events
{
    public class MatchEvent : Event
    {
        private Match match;

        public MatchEvent(Match match)
        {
        }

        public Match Match
        {
            get
            {
                return match;
            }
        }
    }
}