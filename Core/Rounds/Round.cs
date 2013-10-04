using System;
using System.Collections.Generic;
using MBC.Core.Events;
using MBC.Shared;

namespace MBC.Core.Rounds
{
    public class Round : EventCollector, IEventActor
    {
        protected internal Dictionary<IDNumber, FieldInfo> playerField;
        protected internal Dictionary<IDNumber, Register> registers;
        protected internal Dictionary<IDNumber, Team> roundTeams;
        private Dictionary<Type, Action<Event>> eventActions;

        public Round()
        {
            playerField = new Dictionary<IDNumber, FieldInfo>();
            registers = new Dictionary<IDNumber, Register>();
            roundTeams = new Dictionary<IDNumber, Team>();
            eventActions = new Dictionary<Type, Action<Event>>();

            eventActions.Add(typeof(MatchAddPlayerEvent), MatchAddPlayer);
            eventActions.Add(typeof(MatchRemovePlayerEvent), MatchRemovePlayer);
            eventActions.Add(typeof(MatchTeamCreateEvent), MatchTeamCreate);
            eventActions.Add(typeof(RoundBeginEvent), RoundBegin);
        }

        public event MBCEventHandler EventCreated;

        public IDNumber ID
        {
            get;
            private set;
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        public abstract void Play();

        public abstract void Stop();

        protected internal virtual void ReflectEvent(Event ev)
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
            playerField[addPlayerEvent.PlayerID] = new FieldInfo();
            registers[addPlayerEvent.PlayerID] = new Register(addPlayerEvent.PlayerID, addPlayerEvent.PlayerName);
        }

        private void MatchRemovePlayer(Event ev)
        {
            var removePlayerEvent = (MatchRemovePlayerEvent)ev;
            playerField.Remove(removePlayerEvent.PlayerID);
            registers.Remove(removePlayerEvent.PlayerID);
            foreach (var team in roundTeams)
            {
                team.Value.Members.Add(removePlayerEvent.PlayerID);
            }
        }

        private void MatchTeamCreate(Event ev)
        {
            var teamCreateEvent = (MatchTeamCreateEvent)ev;
            roundTeams.Add(teamCreateEvent.TeamID, new Team(teamCreateEvent.TeamID, teamCreateEvent.TeamName));
        }

        private void RoundBegin(Event ev)
        {
            ID = ((RoundBeginEvent)ev).RoundID;
        }
    }
}