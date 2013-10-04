using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MBC.Core.Events;
using MBC.Core.Players;
using MBC.Core.Rounds;
using MBC.Shared;

namespace MBC.Core
{
    /// <summary>
    /// Completely event-driven
    /// </summary>
    public class MatchData
    {
        private bool appendRunToEnd;
        private int currentEventIdx;
        private List<Event> events;
        private int initialMillis;
        private Dictionary<IDNumber, Player> players;
        private Dictionary<IDNumber, RoundData> rounds;
        private List<RoundData> runningRounds;
        private Dictionary<IDNumber, Team> teams;

        private MatchData(SerializationInfo info, StreamingContext context)
        {
        }

        public MatchConfig CompiledConfig
        {
            get;
            private set;
        }

        public IDNumber MatchID
        {
            get;
            private set;
        }

        public IList<IPlayer> Players
        {
            get
            {
                return players.Values.ToList();
            }
        }

        public IList<Team> Teams
        {
            get
            {
                return teams.Values.ToList();
            }
        }

        public void AppendEvent(Event ev)
        {
        }

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}