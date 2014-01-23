using MBC.Core.Events;
using MBC.Core.Rounds;
using MBC.Core.Util;
using MBC.Shared;
using System;
using System.Collections.Generic;
using System.IO;

namespace MBC.Core.Matches
{
    public partial class Match
    {
        private Dictionary<IDNumber, FieldInfo> currentFields;
        private Dictionary<Type, List<MBCEventHandler>> eventActions;
        private Dictionary<IDNumber, Register> registers;
        private Dictionary<IDNumber, Team> teams;

        [Obsolete]
        public Match()
        {
            registers = new Dictionary<IDNumber, Register>();
            teams = new Dictionary<IDNumber, Team>();
            currentFields = new Dictionary<IDNumber, FieldInfo>();
            eventActions = new Dictionary<Type, List<MBCEventHandler>>();
            ID = (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            eventActions[typeof(Event)] = new List<MBCEventHandler>();

            AddEventAction(typeof(MatchBeginEvent), MatchBegin);
            AddEventAction(typeof(MatchAddPlayerEvent), MatchAddPlayer);
            AddEventAction(typeof(MatchConfigChangedEvent), MatchConfigChanged);
            AddEventAction(typeof(MatchTeamCreateEvent), MatchTeamCreate);
            AddEventAction(typeof(MatchRemovePlayerEvent), MatchRemovePlayer);
            AddEventAction(typeof(PlayerShipDestroyedEvent), PlayerShipDestroyed);
            AddEventAction(typeof(PlayerShipsPlacedEvent), PlayerShipsPlaced);
            AddEventAction(typeof(PlayerShotEvent), PlayerShot);
            AddEventAction(typeof(PlayerTeamAssignEvent), MatchPlayerAssign);
            AddEventAction(typeof(PlayerTeamUnassignEvent), MatchPlayerUnassign);
            AddEventAction(typeof(PlayerWonEvent), PlayerWon);
            AddEventAction(typeof(RoundBeginEvent), RoundBegin);
        }

        [Obsolete]
        public MatchConfig CompiledConfig
        {
            get;
            private set;
        }

        [Obsolete]
        public IDictionary<IDNumber, FieldInfo> Fields
        {
            get
            {
                return currentFields;
            }
        }

        public IDNumber ID
        {
            get;
            private set;
        }

        [Obsolete]
        public IDictionary<IDNumber, Register> Registers
        {
            get
            {
                return registers;
            }
        }

        [Obsolete]
        public IDictionary<IDNumber, Team> Teams
        {
            get
            {
                return teams;
            }
        }

        [Obsolete]
        public void AddEventAction(Type typeOfEvent, MBCEventHandler eventAction)
        {
            if (typeOfEvent == typeof(Event))
            {
                eventActions[typeof(Event)].Add(eventAction);
                return;
            }
            Type baseType = typeOfEvent.BaseType;
            while (baseType != null)
            {
                if (baseType == typeof(Event))
                {
                    if (!eventActions.ContainsKey(typeOfEvent))
                    {
                        eventActions[typeOfEvent] = new List<MBCEventHandler>();
                    }
                    eventActions[typeOfEvent].Add(eventAction);
                    return;
                }
                baseType = baseType.BaseType;
            }
            throw new ArgumentException("The type of event must have a base type of Event.");
        }

        [Obsolete]
        public void RemoveEventAction(Type typeOfEvent, MBCEventHandler eventAction)
        {
            if (!eventActions.ContainsKey(typeOfEvent))
            {
                return;
            }
            eventActions[typeOfEvent].Remove(eventAction);
        }

        [Obsolete]
        public virtual void SaveToFile(string location)
        {
        }

        [Obsolete]
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
            foreach (var action in eventActions[typeof(Event)])
            {
                action(ev);
            }
        }

        [Obsolete]
        private void MatchAddPlayer(Event ev)
        {
            var addPlayerEvent = (MatchAddPlayerEvent)ev;
            currentFields[addPlayerEvent.PlayerID] = new FieldInfo();
            registers[addPlayerEvent.PlayerID] = new Register(addPlayerEvent.PlayerID, addPlayerEvent.PlayerName);
        }

        [Obsolete]
        private void MatchBegin(Event ev)
        {
            ID = ((MatchBeginEvent)ev).MatchID;
        }

        [Obsolete]
        private void MatchConfigChanged(Event ev)
        {
            CompiledConfig = ((MatchConfigChangedEvent)ev).Config;
        }

        [Obsolete]
        private void MatchPlayerAssign(Event ev)
        {
            var assignEvent = (PlayerTeamAssignEvent)ev;

            teams[assignEvent.TeamID].Members.Add(assignEvent.PlayerID);
        }

        [Obsolete]
        private void MatchPlayerUnassign(Event ev)
        {
            var unassignEvent = (PlayerTeamUnassignEvent)ev;

            teams[unassignEvent.TeamID].Members.Remove(unassignEvent.PlayerID);
        }

        [Obsolete]
        private void MatchRemovePlayer(Event ev)
        {
            var removeEvent = (MatchRemovePlayerEvent)ev;

            registers.Remove(removeEvent.PlayerID);
            foreach (var team in teams)
            {
                team.Value.Members.Remove(removeEvent.PlayerID);
            }
        }

        [Obsolete]
        private void MatchTeamCreate(Event ev)
        {
            var teamCreateEvent = (MatchTeamCreateEvent)ev;
            teams[teamCreateEvent.Team.ID] = new Team(teamCreateEvent.Team.ID, teamCreateEvent.Team.Name);
        }

        [Obsolete]
        private void PlayerShipDestroyed(Event ev)
        {
            var shipDestroyed = (PlayerShipDestroyedEvent)ev;
            currentFields[shipDestroyed.Player].ShipsLeft.Remove(shipDestroyed.DestroyedShip);
        }

        [Obsolete]
        private void PlayerShipsPlaced(Event ev)
        {
            var shipsPlaced = (PlayerShipsPlacedEvent)ev;
            currentFields[shipsPlaced.Player].Ships = shipsPlaced.Ships;
            currentFields[shipsPlaced.Player].ShipsLeft = shipsPlaced.Ships;
        }

        [Obsolete]
        private void PlayerShot(Event ev)
        {
            var playerShot = (PlayerShotEvent)ev;
            currentFields[playerShot.Player].Shots.Add(playerShot.Shot);
            currentFields[playerShot.Shot.Receiver].ShotsAgainst.Add(playerShot.Shot);
        }

        [Obsolete]
        private void PlayerWon(Event ev)
        {
            registers[((PlayerWonEvent)ev).Player].Score++;
        }

        [Obsolete]
        private void RoundBegin(Event ev)
        {
            foreach (var reg in Registers)
            {
                currentFields[reg.Key] = new FieldInfo();
            }
        }
    }
}