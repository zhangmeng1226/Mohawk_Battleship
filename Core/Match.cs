using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using MBC.Core.Events;
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
    [Configuration("mbc_match_teams", 1)]
    [Configuration("mbc_match_rounds_mode", RoundMode.AllRounds,
        Description = "Determines the ending behaviour of a match based on a given number of rounds.",
        DisplayName = "Match Rounds Mode")]
    [Configuration("mbc_match_rounds", 100)]
    public class Match : ISerializable
    {
        private List<Event> events;
        private List<Round> rounds;
        private List<IPlayer> players;
        private Dictionary<IDNumber, IPlayer> playersByID;

        private int currentEventIdx;
        private FuncThreader matchThreader;

        public Match(Configuration conf)
        {
            Init();
            EnsureGameModeCompatibility();
            SetConfiguration(conf);
        }

        private Match(SerializationInfo info, StreamingContext context)
        {

        }

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        public Match()
        {
            Init();
        }

        public event MBCEventHandler EventCreated;

        /// <summary>
        /// Gets the <see cref="Configuration"/> used to determine game behaviour.
        /// </summary>
        public Configuration Config
        {
            get;
            private set;
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        public MatchConfig CompiledConfig
        {
            get;
            private set;
        }

        public IDNumber ID
        {
            get;
            private set;
        }

        public IList<IPlayer> Players
        {
            get
            {
                return players.AsReadOnly();
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

        private IList<Round> Rounds
        {
            get
            {
                return rounds.AsReadOnly();
            }
        }

        public void AddController(ControlledPlayer plr)
        {
            if (rounds.Count > 0 && Config.GetValue<bool>("mbc_match_playeradd_init_only"))
            {
                throw new InvalidOperationException("Cannot add players after the match has started (mbc_match_playeradd_init_only set to true)");
            }
            for (int i = 0; i < playersByID.Count + 1; i++)
            {
                if (!playersByID.ContainsKey(i))
                {
                    playersByID[i] = plr;
                    plr.NewMatch(CompiledConfig, i);
                    players.Add(plr);
                    EventCreated(new MatchAddPlayerEvent(plr.Register.ID, plr.Register.Name));
                    return;
                }
            }
        }

        public void End()
        {
            matchThreader.Stop();
            EventCreated(new MatchEndEvent());
        }

        public IPlayer GetPlayerByID(IDNumber id)
        {
            return playersByID[id];
        }

        public void AddEvent(Event newEvent)
        {
            events.Add(newEvent);
        }

        public void PlayThread()
        {
            while (IsRunning && !MatchConditionsMet())
            {

            }
        }

        public void SaveToFile(File fLocation)
        {
        }

        private Round CreateNewRound()
        {
            foreach (var mode in Config.GetList<GameMode>("mbc_game_mode"))
            {
                switch (mode)
                {
                    case GameMode.Classic:
                        return new ClassicRound(this);
                }
            }
            throw new InvalidOperationException("An unsupported game mode was configured for this match!");
        }

        private void EnsureGameModeCompatibility()
        {
            foreach (var mode in Config.GetList<GameMode>("mbc_game_mode"))
            {
                if (mode == GameMode.Teams)
                {
                    throw new NotImplementedException("The " + mode.ToString() + " game mode is not supported.");
                }
            }
        }

        private void Init()
        {
            EventCreated += AddEvent;
            events = new List<Event>();
            rounds = new List<Round>();
            players = new List<IPlayer>();
            playersByID = new Dictionary<IDNumber, IPlayer>();
            matchThreader = new FuncThreader(new Action(PlayThread));
            ID = (int)DateTime.Now.Subtract(new DateTime(1970,1,1,0,0,0)).TotalSeconds;
            EventCreated(new MatchBeginEvent(ID));
        }

        private bool MatchConditionsMet()
        {
            switch (CompiledConfig.RoundMode)
            {
                case RoundMode.AllRounds:
                    return rounds.Count >= CompiledConfig.NumberOfRounds;
                case RoundMode.FirstTo:
                    foreach (var player in players)
                    {
                        if (player.Register.Score >= CompiledConfig.NumberOfRounds)
                        {

                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        public void SetConfiguration(Configuration config)
        {
            Config = config;
            CompiledConfig = new MatchConfig();
            CompiledConfig.FieldSize = new Coordinates(Config.GetValue<int>("mbc_field_width"), Config.GetValue<int>("mbc_field_height"));
            CompiledConfig.NumberOfRounds = Config.GetValue<int>("mbc_match_rounds");

            CompiledConfig.Registers = new Dictionary<IDNumber, Register>();
            foreach (var player in players)
            {
                CompiledConfig.Registers.Add(player.Register.ID, player.Register);
            }

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
            EventCreated(new MatchConfigChangedEvent(CompiledConfig));
        }
    }
}