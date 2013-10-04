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
            SetConfiguration(conf);
            GenerateEvent(new MatchBeginEvent(ID));
        }

        public ActiveMatch()
        {
            SetConfiguration(Configuration.Global);
            GenerateEvent(new MatchBeginEvent(ID));
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
                    GenerateEvent(new MatchAddPlayerEvent(plr.Register.ID, plr.Register.Name));
                    return;
                }
            }
        }

        public void End()
        {
            Stop();
            GenerateEvent(new MatchEndEvent());
        }

        public override void Play()
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
            CompiledConfig = new MatchConfig();
            CompiledConfig.FieldSize = new Coordinates(Config.GetValue<int>("mbc_field_width"), Config.GetValue<int>("mbc_field_height"));
            CompiledConfig.NumberOfRounds = Config.GetValue<int>("mbc_match_rounds");

            CompiledConfig.StartingShips = new ShipList();
            foreach (var length in Config.GetList<int>("mbc_ship_sizes"))
            {
                CompiledConfig.StartingShips.Add(new Ship(length));
            }

            CompiledConfig.TimeLimit = Config.GetValue<int>("mbc_player_thread_timeout");

            CompiledConfig.GameMode = 0;
            foreach (var mode in Config.GetList<GameMode>("mbc_game_mode"))
            {
                CompiledConfig.GameMode |= mode;
            }
            if (!CompiledConfig.GameMode.HasFlag(GameMode.Classic))
            {
                throw new NotImplementedException("The " + CompiledConfig.GameMode.ToString() + " game mode is not supported.");
            }
            GenerateEvent(new MatchConfigChangedEvent(CompiledConfig));
        }

        public override void Stop()
        {
            IsPlaying = false;
        }

        protected virtual Round CreateNewRound()
        {
            if (CompiledConfig.GameMode.HasFlag(GameMode.Classic))
            {
                return new ClassicRound(this);
            }
        }
    }
}