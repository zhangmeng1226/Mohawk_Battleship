using System;
using System.Collections.Generic;
using System.IO;
using MBC.Core.Events;
using MBC.Core.Rounds;
using MBC.Core.Util;
using MBC.Shared;

namespace MBC.Core.Matches
{
    public class ActiveMatch : Match
    {
        private Dictionary<IDNumber, ControlledPlayer> controllers;
        private Dictionary<IDNumber, Round> progressRounds;

        public ActiveMatch(Configuration conf)
        {
            controllers = new Dictionary<IDNumber, ControlledPlayer>();
            progressRounds = new Dictionary<IDNumber, Round>();

            ApplyEvent(new MatchBeginEvent(ID));
            SetConfiguration(conf);

            AddEventAction(typeof(MatchAddPlayerEvent), MatchAddPlayer);
            AddEventAction(typeof(MatchRemovePlayerEvent), MatchRemovePlayer);
            AddEventAction(typeof(PlayerTeamAssignEvent), PlayerTeamAssign);
            AddEventAction(typeof(PlayerTeamUnassignEvent), PlayerTeamUnassign);
            AddEventAction(typeof(MatchTeamCreateEvent), MatchTeamCreate);
        }

        public ActiveMatch()
            : this(Configuration.Global)
        {
        }

        public Configuration Config
        {
            get;
            private set;
        }

        public virtual void AddController(ControlledPlayer plr)
        {
            for (int i = 0; i < controllers.Count + 1; i++)
            {
                if (!controllers.ContainsKey(i))
                {
                    controllers[i] = plr;
                    plr.NewMatch();
                    ApplyEvent(new MatchAddPlayerEvent(i, plr.Skeleton.Name));
                    return;
                }
            }
        }

        public void End()
        {
            Stop();
            ApplyEvent(new MatchEndEvent());
        }

        public virtual void Play()
        {
            IsPlaying = true;
            while (IsPlaying)
            {
            }
        }

        public override void SaveToFile(File fLocation)
        {
            throw new NotImplementedException();
        }

        public void SetConfiguration(Configuration config)
        {
            Config = config;
            var newConfig = new MatchConfig();
            newConfig.FieldSize = new Coordinates(Config.GetValue<int>("mbc_field_width"), Config.GetValue<int>("mbc_field_height"));
            newConfig.NumberOfRounds = Config.GetValue<int>("mbc_match_rounds");

            newConfig.StartingShips = new ShipList();
            foreach (var length in Config.GetList<int>("mbc_ship_sizes"))
            {
                newConfig.StartingShips.Add(new Ship(length));
            }

            newConfig.TimeLimit = Config.GetValue<int>("mbc_player_thread_timeout");

            newConfig.GameMode = 0;
            foreach (var mode in Config.GetList<GameMode>("mbc_game_mode"))
            {
                newConfig.GameMode |= mode;
            }
            if (!newConfig.GameMode.HasFlag(GameMode.Classic))
            {
                throw new NotImplementedException("The " + newConfig.GameMode.ToString() + " game mode is not supported.");
            }
            ApplyEvent(new MatchConfigChangedEvent(newConfig));
        }

        public virtual void SetControllerToTeam(IDNumber ctrl, IDNumber team)
        {
            if (!controllers.ContainsKey(ctrl))
            {
                throw new ArgumentException("Controller ID number " + ctrl + " does not exist in the current match.");
            }
            if (!Teams.ContainsKey(team))
            {
                throw new ArgumentException("Team ID number " + team + " does not exist in the current match.");
            }
            ApplyEvent(new PlayerTeamAssignEvent(ctrl, team));
        }

        public virtual void Stop()
        {
            IsPlaying = false;
        }

        public virtual void UnsetControllerFromTeam(IDNumber ctrl, IDNumber team)
        {
            if (!controllers.ContainsKey(ctrl))
            {
                throw new ArgumentException("Controller ID number " + ctrl + " does not exist in the current match.");
            }
            if (!Teams.ContainsKey(team))
            {
                throw new ArgumentException("Team ID number " + team + " does not exist in the current match.");
            }
            ApplyEvent(new PlayerTeamUnassignEvent(ctrl, team));
        }

        private void ControllersUpdateRegisters()
        {
            foreach (var controller in controllers)
            {
                controller.Value.Registers = (Dictionary<IDNumber, Register>)Registers;
            }
        }

        private void ControllersUpdateTeams()
        {
            foreach (var controller in controllers)
            {
                controller.Value.Teams = (Dictionary<IDNumber, Team>)Teams;
            }
        }

        private void MatchAddPlayer(Event ev)
        {
            var addPlayer = (MatchAddPlayerEvent)ev;
            ControllersUpdateRegisters();
        }

        private void MatchRemovePlayer(Event ev)
        {
            var removePlayer = (MatchAddPlayerEvent)ev;
            controllers.Remove(removePlayer.PlayerID);
            ControllersUpdateRegisters();
        }

        private void MatchTeamCreate(Event ev)
        {
            var teamCreate = (MatchTeamCreateEvent)ev;
            ControllersUpdateTeams();
        }

        private void PlayerTeamAssign(Event ev)
        {
            throw new NotImplementedException();
        }

        private void PlayerTeamUnassign(Event ev)
        {
            throw new NotImplementedException();
        }
    }
}