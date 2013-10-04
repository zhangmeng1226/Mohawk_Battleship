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
    public abstract class Match : EventCollector, IEventActor
    {
        protected internal Dictionary<IDNumber, Player> players;
        protected internal Dictionary<IDNumber, Round> rounds;
        protected internal Dictionary<IDNumber, Team> teams;
        private Dictionary<Type, Action<Event>> eventActions;
        private FuncThreader matchThreader;

        public Match()
        {
            players = new Dictionary<IDNumber, Player>();
            rounds = new Dictionary<IDNumber, Round>();
            teams = new Dictionary<IDNumber, Team>();
            eventActions = new Dictionary<Type, Action<Event>>();
            matchThreader = new FuncThreader(new Action(Play));
            ID = (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

            eventActions.Add(typeof(MatchBeginEvent), MatchBegin);
            eventActions.Add(typeof(MatchAddPlayerEvent), MatchAddPlayer);
            eventActions.Add(typeof(MatchConfigChangedEvent), MatchConfigChanged);
            eventActions.Add(typeof(MatchPlayerAssignEvent), MatchPlayerAssign);
            eventActions.Add(typeof(MatchTeamCreateEvent), MatchTeamCreate);
            eventActions.Add(typeof(MatchRemovePlayerEvent), MatchRemovePlayer);
            eventActions.Add(typeof(RoundBeginEvent), RoundBegin);
        }

        public MatchConfig CompiledConfig
        {
            get;
            protected internal set;
        }

        public IDNumber ID
        {
            get;
            protected internal set;
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

        public IDictionary<IDNumber, Round> Rounds
        {
            get
            {
                return rounds;
            }
        }

        public IDictionary<IDNumber, Team> Teams
        {
            get
            {
                return teams;
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

        protected virtual void ReflectEvent(Event ev)
        {
            var eventType = ev.GetType();
            if (eventActions.ContainsKey(eventType))
            {
                eventActions[eventType](ev);
            }
        }

        private void MatchAddPlayer(Event ev)
        {
            var addPlayerEvent = (MatchAddPlayerEvent)ev;
            var newPlayer = new Player(addPlayerEvent.PlayerID, addPlayerEvent.PlayerName);
            players[addPlayerEvent.PlayerID] = newPlayer;
        }

        private void MatchBegin(Event ev)
        {
            ID = ((MatchBeginEvent)ev).MatchID;
        }

        private void MatchConfigChanged(Event ev)
        {
            CompiledConfig = ((MatchConfigChangedEvent)ev).Config;
        }

        private void MatchPlayerAssign(Event ev)
        {
            var assignEvent = (MatchPlayerAssignEvent)ev;
            var plr = players[assignEvent.PlayerID];
            var team = teams[assignEvent.TeamID];

            if (plr.Team != null)
            {
                plr.Team.Members.Remove(assignEvent.PlayerID);
            }

            team.Members.Add(assignEvent.PlayerID);
            plr.Team = team;
        }

        private void MatchRemovePlayer(Event ev)
        {
            var removeEvent = (MatchRemovePlayerEvent)ev;

            players[removeEvent.PlayerID].Team.Members.Remove(removeEvent.PlayerID);
            players.Remove(removeEvent.PlayerID);
        }

        private void MatchTeamCreate(Event ev)
        {
            MatchTeamCreateEvent teamCreateEvent = (MatchTeamCreateEvent)ev;
            teams[teamCreateEvent.TeamID] = new Team(teamCreateEvent.TeamID, teamCreateEvent.TeamName);
        }

        private void RoundBegin(Event ev)
        {
            rounds.Add(((RoundBeginEvent)ev).RoundID, new Round());
        }
    }
}