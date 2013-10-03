using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
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
        private List<Event> events;
        private Dictionary<IDNumber, Player> players;
        private Dictionary<IDNumber, RoundData> rounds;
        private Dictionary<IDNumber, Team> teams;
        private List<RoundData> runningRounds;

        private int currentEventIdx;
        private int initialMillis;
        private bool appendRunToEnd;

        private MatchData(SerializationInfo info, StreamingContext context)
        {

        }

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {

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

        public IDNumber MatchID
        {
            get;
            private set;
        }

        public MatchConfig CompiledConfig
        {
            get;
            private set;
        }

        public void AppendEvent(Event ev)
        {

        }

    }
}
