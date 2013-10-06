using System;
using System.Collections.Generic;
using MBC.Core.Events;
using MBC.Shared;

namespace MBC.Core.Rounds
{
    public abstract class Round
    {
        private Dictionary<Type, List<MBCEventHandler>> eventActions;
        private EventDriver eventDriver;
        private Dictionary<IDNumber, FieldInfo> playerField;
        private Dictionary<IDNumber, Register> registers;
        private Dictionary<IDNumber, Team> roundTeams;

        public Round()
        {
            playerField = new Dictionary<IDNumber, FieldInfo>();
            registers = new Dictionary<IDNumber, Register>();
            roundTeams = new Dictionary<IDNumber, Team>();
            eventActions = new Dictionary<Type, List<MBCEventHandler>>();
            eventDriver = new EventDriver();
            eventDriver.EventApplied += ReflectEvent;

            AddEventAction(typeof(MatchAddPlayerEvent), MatchAddPlayer);
            AddEventAction(typeof(MatchRemovePlayerEvent), MatchRemovePlayer);
            AddEventAction(typeof(MatchTeamCreateEvent), MatchTeamCreate);
            AddEventAction(typeof(RoundBeginEvent), RoundBegin);
        }

        public IDNumber ID
        {
            get;
            private set;
        }

        public IDictionary<IDNumber, FieldInfo> PlayerField
        {
            get
            {
                return playerField;
            }
        }

        public IDictionary<IDNumber, Register> Registers
        {
            get
            {
                return registers;
            }
        }

        public IDictionary<IDNumber, Team> Teams
        {
            get
            {
                return roundTeams;
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