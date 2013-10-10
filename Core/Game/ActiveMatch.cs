using System;
using System.Collections.Generic;
using System.IO;
using MBC.Core.Controllers;
using MBC.Core.Events;
using MBC.Core.Rounds;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;

namespace MBC.Core.Matches
{
    public abstract class ActiveMatch : Match
    {
        protected Dictionary<IDNumber, IController> controllers;

        public ActiveMatch(Configuration conf)
        {
            Events = new EventDriver();
            Events.EventApplied += ReflectEvent;
            controllers = new Dictionary<IDNumber, IController>();

            ApplyEvent(new MatchBeginEvent(ID));
            SetConfiguration(conf);

            AddEventAction(typeof(MatchAddPlayerEvent), MatchAddPlayer);
            AddEventAction(typeof(MatchRemovePlayerEvent), MatchRemovePlayer);
            AddEventAction(typeof(PlayerTeamAssignEvent), TeamChangeEvent);
            AddEventAction(typeof(PlayerTeamUnassignEvent), TeamChangeEvent);
            AddEventAction(typeof(MatchTeamCreateEvent), TeamChangeEvent);
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

        public IDictionary<IDNumber, IController> Controllers
        {
            get
            {
                return controllers;
            }
        }

        public abstract bool Ended { get; }

        public EventDriver Events
        {
            get;
            private set;
        }

        public virtual void AddController(IController plr)
        {
            for (int i = 0; i < controllers.Count + 1; i++)
            {
                if (!controllers.ContainsKey(i))
                {
                    controllers[i] = plr;
                    plr.NewMatch();
                    plr.Match = new MatchConfig(CompiledConfig);
                    ApplyEvent(new MatchAddPlayerEvent(i, plr.GetAttribute<NameAttribute>().ToString()));
                    return;
                }
            }
        }

        public void End()
        {
            Stop();
            ApplyEvent(new MatchEndEvent());
        }

        public IDNumber GetTeam(string name)
        {
            return GetTeam(name, false);
        }

        public override void SaveToFile(string location)
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

            newConfig.TimeLimit = Config.GetValue<int>("mbc_player_timeout");

            newConfig.GameMode = 0;
            foreach (var mode in Config.GetList<GameMode>("mbc_game_mode"))
            {
                newConfig.GameMode |= mode;
            }
            if (!newConfig.GameMode.HasFlag(GameMode.Classic))
            {
                throw new NotImplementedException("The " + newConfig.GameMode.ToString() + " game mode is not supported.");
            }
            newConfig.Random = new Random();
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

        protected internal void ApplyEvent(Event ev)
        {
            Events.ApplyEvent(ev);
        }

        protected internal IDNumber GetTeam(string name, bool internalTeam)
        {
            foreach (var team in Teams)
            {
                if (team.Value.Name == name)
                {
                    team.Value.Members.Clear();
                    return team.Key;
                }
            }
            for (int i = 0; i <= Teams.Count; i++)
            {
                if (!Teams.ContainsKey(i))
                {
                    ApplyEvent(new MatchTeamCreateEvent(new Team(i, name, internalTeam)));
                    return i;
                }
            }
            throw new InvalidProgramException("Not supposed to happen.");
        }

        private void ControllersUpdateMatch()
        {
            foreach (var ctrl in controllers)
            {
                ctrl.Value.Match = new MatchConfig(CompiledConfig);
            }
        }

        private void ControllersUpdateRegisters()
        {
            if (Events.AtEnd)
            {
                foreach (var controller in controllers)
                {
                    controller.Value.Registers = new Dictionary<IDNumber, Register>();
                    controller.Value.ID = controller.Key;
                    foreach (var reg in Registers)
                    {
                        controller.Value.Registers.Add(reg.Key, new Register(reg.Value));
                    }
                }
            }
        }

        private void ControllersUpdateTeams()
        {
            if (Events.AtEnd)
            {
                foreach (var controller in controllers)
                {
                    controller.Value.Teams = new Dictionary<IDNumber, Team>();
                    foreach (var team in Teams)
                    {
                        controller.Value.Teams.Add(team.Key, new Team(team.Value));
                    }
                }
            }
        }

        private void MatchAddPlayer(Event ev)
        {
            var playerEvent = (MatchAddPlayerEvent)ev;
            Controllers[playerEvent.PlayerID].Match = new MatchConfig(CompiledConfig);
            ControllersUpdateRegisters();
            ControllersUpdateTeams();
        }

        private void MatchRemovePlayer(Event ev)
        {
            var removePlayer = (MatchAddPlayerEvent)ev;
            ControllersUpdateRegisters();
            ControllersUpdateTeams();
        }

        private void TeamChangeEvent(Event ev)
        {
            ControllersUpdateTeams();
        }
    }
}