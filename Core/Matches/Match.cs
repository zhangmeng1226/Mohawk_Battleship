using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using MBC.Core.Events;
using MBC.Core.Players;
using MBC.Core.Rounds;
using MBC.Core.Threading;
using MBC.Core.Util;
using MBC.Shared;

namespace MBC.Core.Matches
{
    [Configuration("mbc_field_width", 10)]
    [Configuration("mbc_field_height", 10)]
    [Configuration("mbc_ship_sizes", 2, 3, 3, 4, 5)]
    [Configuration("mbc_game_mode", GameMode.Classic, null)]
    [Configuration("mbc_match_playeradd_init_only", true)]
    [Configuration("mbc_match_rounds_mode", RoundMode.AllRounds,
        Description = "Determines the ending behaviour of a match based on a given number of rounds.",
        DisplayName = "Match Rounds Mode")]
    [Configuration("mbc_match_rounds", 100)]
    public abstract class Match : ISerializable
    {
        private int currentEventIdx;
        private List<Event> events;

        private Dictionary<IDNumber, Player> players;
        private Dictionary<IDNumber, Round> rounds;
        private Dictionary<IDNumber, Team> teams;

        private FuncThreader matchThreader;

        public Match()
        {
            ID = (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        private Match(SerializationInfo info, StreamingContext context)
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        public virtual void AddPlayer(Player plr)
        {

        }

        public event MBCEventHandler EventCreated;
        public MatchConfig CompiledConfig
        {
            get;
            protected set;
        }

        public IDNumber ID
        {
            get;
            private set;
        }

        public bool IsPlaying
        {
            get;
            protected set;
        }
        public IDictionary<IDNumber, Player> Players
        {
            get
            {
                return players;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Event"/>s that have been generated.
        /// </summary>
        private IList<Event> Events
        {
            get
            {
                return events.AsReadOnly();
            }
        }

        private IDictionary<IDNumber, Round> Rounds
        {
            get
            {
                return rounds;
            }
        }

        public abstract void Play();

        public void PlayThread()
        {
            matchThreader.Run();
        }

        public void SaveToFile(File fLocation)
        {

        }

        public abstract void Stop();

        protected void GenerateEvent(Event ev)
        {
            events.Add(ev);
            if (EventCreated != null)
            {
                EventCreated(ev);
            }
        }

        protected virtual void ReflectEvent(Event ev)
        {
            switch (ev.EventType)
            {
                case Event.Type.MatchBegin:
                    ID = ((MatchBeginEvent)ev).MatchID;
                    break;

                case Event.Type.MatchConfigChanged:
                    CompiledConfig = ((MatchConfigChangedEvent)ev).Config;
                    foreach (var player in players)
                    {
                        player.Value.Match = CompiledConfig;
                    }
                    teams = new Dictionary<IDNumber, Team>();
                    break;

                case Event.Type.MatchAddPlayer:
                    var addPlayerEvent = (MatchAddPlayerEvent)ev;
                    Player newPlayer = new Player(addPlayerEvent.PlayerID, addPlayerEvent.PlayerName);
                    newPlayer.Match = CompiledConfig;
                    newPlayer.Register.ID = addPlayerEvent.PlayerID;
                    players[addPlayerEvent.PlayerID] = newPlayer;
                    foreach (var round in rounds)
                    {
                        round.Value.ReflectEvent(ev);
                    }
                    break;

                case Event.Type.MatchRemovePlayer:
                    players.Remove(((MatchRemovePlayerEvent)ev).PlayerID);
                    foreach (var round in rounds)
                    {
                        round.Value.ReflectEvent(ev);
                    }
                    break;

                case Event.Type.MatchTeamCreate:
                    MatchTeamCreateEvent teamCreateEvent = (MatchTeamCreateEvent)ev;
                    teams[teamCreateEvent.TeamID] = new Team(teamCreateEvent.TeamID, teamCreateEvent.TeamName);
                    break;

                case Event.Type.MatchPlayerTeamAssign:
                    break;
            }
        }
    }
}