using System;
using System.Collections.Generic;
using System.IO;
using MBC.Core.Events;
using MBC.Core.Rounds;
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
        private Dictionary<IDNumber, FieldInfo> currentFields;
        private Dictionary<Type, List<MBCEventHandler>> eventActions;
        private Dictionary<IDNumber, Register> registers;
        private Dictionary<IDNumber, Team> teams;

        public Match()
        {
            registers = new Dictionary<IDNumber, Register>();
            teams = new Dictionary<IDNumber, Team>();
            currentFields = new Dictionary<IDNumber, FieldInfo>();
            eventActions = new Dictionary<Type, List<MBCEventHandler>>();
            ID = (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

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

        public MatchConfig CompiledConfig
        {
            get;
            private set;
        }

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
                return teams;
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

        public abstract void Play();

        public void RemoveEventAction(Type typeOfEvent, MBCEventHandler eventAction)
        {
            if (!eventActions.ContainsKey(typeOfEvent))
            {
                return;
            }
            eventActions[typeOfEvent].Remove(eventAction);
        }

        public abstract void SaveToFile(string location);

        public abstract void Stop();

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
            currentFields[addPlayerEvent.PlayerID] = new FieldInfo();
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

            teams[unassignEvent.TeamID].Members.Remove(unassignEvent.PlayerID);
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
            teams[teamCreateEvent.Team.ID] = new Team(teamCreateEvent.Team.ID, teamCreateEvent.Team.Name);
        }

        private void PlayerShipDestroyed(Event ev)
        {
            var shipDestroyed = (PlayerShipDestroyedEvent)ev;
            currentFields[shipDestroyed.Player].ShipsLeft.Remove(shipDestroyed.DestroyedShip);
        }

        private void PlayerShipsPlaced(Event ev)
        {
            var shipsPlaced = (PlayerShipsPlacedEvent)ev;
            currentFields[shipsPlaced.Player].Ships = shipsPlaced.Ships;
            currentFields[shipsPlaced.Player].ShipsLeft = shipsPlaced.Ships;
        }

        private void PlayerShot(Event ev)
        {
            var playerShot = (PlayerShotEvent)ev;
            currentFields[playerShot.Player].Shots.Add(playerShot.Shot);
            currentFields[playerShot.Shot.Receiver].ShotsAgainst.Add(playerShot.Shot);
        }

        private void PlayerWon(Event ev)
        {
            registers[((PlayerWonEvent)ev).Player].Score++;
        }

        private void RoundBegin(Event ev)
        {
            foreach (var field in currentFields)
            {
                currentFields[field.Key] = new FieldInfo();
            }
        }
    }
}