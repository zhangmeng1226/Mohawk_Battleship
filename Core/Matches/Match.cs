using System;
using System.Collections.Generic;
using System.IO;
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
    public abstract class Match
    {
        private Dictionary<Type, List<MBCEventHandler>> eventActions;
        private EventDriver eventDriver;
        private Dictionary<IDNumber, Register> registers;
        private Dictionary<IDNumber, Round> rounds;
        private Dictionary<IDNumber, Team> teams;

        public Match()
        {
            registers = new Dictionary<IDNumber, Register>();
            rounds = new Dictionary<IDNumber, Round>();
            teams = new Dictionary<IDNumber, Team>();
            eventActions = new Dictionary<Type, List<MBCEventHandler>>();
            eventDriver = new EventDriver();
            eventDriver.EventApplied += ReflectEvent;
            ID = (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

            AddEventAction(typeof(MatchBeginEvent), MatchBegin);
            AddEventAction(typeof(MatchAddPlayerEvent), MatchAddPlayer);
            AddEventAction(typeof(MatchConfigChangedEvent), MatchConfigChanged);
            AddEventAction(typeof(PlayerTeamAssignEvent), MatchPlayerAssign);
            AddEventAction(typeof(PlayerTeamUnassignEvent), MatchPlayerUnassign);
            AddEventAction(typeof(MatchTeamCreateEvent), MatchTeamCreate);
            AddEventAction(typeof(MatchRemovePlayerEvent), MatchRemovePlayer);
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

        public IDictionary<IDNumber, Register> Registers
        {
            get
            {
                return registers;
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

        protected EventDriver EventDriver
        {
            get
            {
                return eventDriver;
            }
        }

        public void AddEventAction(Type typeOfEvent, MBCEventHandler eventAction)
        {
            if (!eventActions.ContainsKey(typeOfEvent))
            {
                eventActions[typeOfEvent] = new List<MBCEventHandler>();
            }
            eventActions[typeOfEvent].Add(eventAction);
        }

        public void SaveToFile(File fLocation)
        {
        }

        protected void ApplyEvent(Event ev)
        {
            eventDriver.ApplyEvent(ev);
        }

        protected virtual void ReflectEvent(Event ev)
        {
            var eventType = ev.GetType();
            if (eventActions.ContainsKey(eventType))
            {
                foreach (MBCEventHandler action in eventActions[eventType])
                {
                    action(ev);
                }
            }
        }

        private void MatchAddPlayer(Event ev)
        {
            var addPlayerEvent = (MatchAddPlayerEvent)ev;
            registers[addPlayerEvent.PlayerID] = new Register(addPlayerEvent.PlayerID, addPlayerEvent.PlayerName);
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
            var assignEvent = (PlayerTeamAssignEvent)ev;

            teams[assignEvent.TeamID].Members.Add(assignEvent.PlayerID);
        }

        private void MatchPlayerUnassign(Event ev)
        {
            var unassignEvent = (PlayerTeamUnassignEvent)ev;

            teams[unassignEvent.TeamID].Members.Add(unassignEvent.PlayerID);
        }

        private void MatchRemovePlayer(Event ev)
        {
            var removeEvent = (MatchRemovePlayerEvent)ev;

            registers.Remove(removeEvent.PlayerID);
            foreach (var team in teams)
            {
                team.Value.Members.Remove(removeEvent.PlayerID);
            }
        }

        private void MatchTeamCreate(Event ev)
        {
            var teamCreateEvent = (MatchTeamCreateEvent)ev;
            teams[teamCreateEvent.TeamID] = new Team(teamCreateEvent.TeamID, teamCreateEvent.TeamName);
        }
    }
}