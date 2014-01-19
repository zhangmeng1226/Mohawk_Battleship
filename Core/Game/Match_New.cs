using MBC.Core.Events;
using MBC.Core.Threading;
using MBC.Core.Util;
using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MBC.Core.Game
{
    public delegate void MatchAddPlayerHandler(Match match, MatchAddPlayerEvent ev);

    public delegate void MatchBeginHandler(Match match, MatchBeginEvent ev);

    public delegate void MatchEndHandler(Match match, MatchEndEvent ev);

    public delegate void MatchParamChangeHandler(Match match, MatchConfigChangedEvent ev);

    public delegate void MatchRemovePlayerHandler(Match match, MatchRemovePlayerEvent ev);

    public delegate void PlayerLostHandler(Match match, PlayerLostEvent ev);

    public delegate void PlayerMessageHandler(Match match, PlayerMessageEvent ev);

    public delegate void PlayerShipDestroyedHandler(Match match, PlayerShipDestroyedEvent ev);

    public delegate void PlayerShipsPlacedHandler(Match match, PlayerShipsPlacedEvent ev);

    public delegate void PlayerShotHandler(Match match, PlayerShotEvent ev);

    public delegate void PlayerTeamAssignHandler(Match match, PlayerTeamAssignEvent ev);

    public delegate void PlayerTimeoutHandler(Match match, PlayerTimeoutEvent ev);

    public delegate void PlayerWonHandler(Match match, PlayerWonEvent ev);

    public delegate void RoundBeginHandler(Match match, RoundBeginEvent ev);

    public delegate void RoundEndHandler(Match match, RoundEndEvent ev);

    public delegate void TeamAddHandler(Match match, TeamAddHandler ev);

    public delegate void TeamRemoveHandler(Match match, TeamRemoveHandler ev);

    /// <summary>
    /// This is the new framework part of a match.
    /// </summary>
    [Configuration("mbc_field_width", 10)]
    [Configuration("mbc_field_height", 10)]
    [Configuration("mbc_ship_sizes", 2, 3, 3, 4, 5)]
    [Configuration("mbc_game_mode", GameMode.Classic, null)]
    [Configuration("mbc_match_playeradd_init_only", true)]
    [Configuration("mbc_match_rounds_mode", RoundMode.AllRounds,
        Description = "Determines the ending behaviour of a match based on a given number of rounds.",
        DisplayName = "Match Rounds Mode")]
    [Configuration("mbc_match_rounds", 100)]
    public partial class Match
    {
        private List<Event> events;
        private Coordinates fieldSize;
        private List<GameMode> gameModes;
        private Stopwatch gameTimer;
        private int numberOfRounds;
        private List<Player> players;
        private Random randObj;
        private RoundMode rndBehavior;
        private List<Ship> startingShips;
        private List<Team> teams;
        private int timeLimit;

        public Match(Configuration config)
        {
            events = new List<Event>();
            players = new List<Player>();
            teams = new List<Team>();
            gameTimer = new Stopwatch();
            randObj = new Random();

            ApplyParameters(config);
        }

        public event MatchParamChangeHandler OnConfigChange;

        public event MatchBeginHandler OnMatchBegin;

        public event MatchEndHandler OnMatchEnd;

        public event MatchAddPlayerHandler OnPlayerAdd;

        public event PlayerLostHandler OnPlayerLose;

        public event PlayerMessageHandler OnPlayerMessage;

        public event MatchRemovePlayerHandler OnPlayerRemove;

        public event PlayerShipDestroyedHandler OnPlayerShipDestruction;

        public event PlayerShipsPlacedHandler OnPlayerShipsPlaced;

        public event PlayerShotHandler OnPlayerShot;

        public event PlayerTeamAssignHandler OnPlayerTeamAssign;

        public event PlayerTimeoutHandler OnPlayerTimeout;

        public event PlayerWonHandler OnPlayerWin;

        public event RoundBeginHandler OnRoundBegin;

        public event RoundEndHandler OnRoundEnd;

        public event TeamAddHandler OnTeamAdd;

        public event TeamRemoveHandler OnTeamRemove;

        public Coordinates FieldSize
        {
            get
            {
                return fieldSize;
            }
            set
            {
                fieldSize = value;
                NotifyParamsChanged();
            }
        }

        public List<GameMode> Modes
        {
            get
            {
                return gameModes;
            }
            set
            {
                gameModes = value;
                NotifyParamsChanged();
            }
        }

        public int NumberOfRounds
        {
            get
            {
                return numberOfRounds;
            }
            set
            {
                numberOfRounds = value;
                NotifyParamsChanged();
            }
        }

        public RoundMode RoundMode
        {
            get
            {
                return rndBehavior;
            }
            set
            {
                rndBehavior = value;
                NotifyParamsChanged();
            }
        }

        public int TimeLimit
        {
            get
            {
                return timeLimit;
            }
            set
            {
                timeLimit = value;
                NotifyParamsChanged();
            }
        }

        public void AddPlayer(Player plr)
        {
            players.Add(plr);
            MatchAddPlayerEvent ev = new MatchAddPlayerEvent(plr);
            AppendEvent(ev);
            if (OnPlayerAdd != null)
            {
                OnPlayerAdd(this, ev);
            }
        }

        public void AddTeam(Team newTeam)
        {
            teams.Add(newTeam);
        }

        public void PlayerLost(Player plr)
        {
            plr.Losses++;
            PlayerLostEvent ev = new PlayerLostEvent(plr);
            AppendEvent(ev);
            if (OnPlayerLose != null)
            {
                OnPlayerLose(this, ev);
            }
        }

        public void PlayerShot(Player plr, Shot shot)
        {
            Ship shipHit = ShipList.GetShipAt(shot.ReceiverPlr.Ships, shot.Coordinates);
            PlayerShotEvent ev = new PlayerShotEvent(plr, shot, shipHit);
            AppendEvent(ev);
            if (OnPlayerShot != null)
            {
                OnPlayerShot(this, ev);
            }
        }

        public void PlayerTeamAssign(Player plr, Team newTeam)
        {
            plr.Team = newTeam;
            PlayerTeamAssignEvent ev = new PlayerTeamAssignEvent(plr, newTeam);
            AppendEvent(ev);
            if (OnPlayerTeamAssign != null)
            {
                OnPlayerTeamAssign(this, ev);
            }
        }

        public void PlayerTimeout(Player plr, ControllerTimeoutException ex)
        {
            PlayerTimeoutEvent ev = new PlayerTimeoutEvent(plr, ex);
            AppendEvent(ev);
            if (OnPlayerTimeout != null)
            {
                OnPlayerTimeout(this, ev);
            }
        }

        public void PlayerWin(Player plr)
        {
            plr.Wins++;
            PlayerWonEvent ev = new PlayerWonEvent(plr);
            AppendEvent(ev);
            if (OnPlayerWin != null)
            {
                OnPlayerWin(this, ev);
            }
        }

        public void RemovePlayer(Player plr)
        {
            players.Remove(plr);
            MatchRemovePlayerEvent ev = new MatchRemovePlayerEvent(plr);
            AppendEvent(ev);
            if (OnPlayerRemove != null)
            {
                OnPlayerRemove(this, ev);
            }
        }

        public void RemoveTeam(Team remTeam)
        {
            teams.Remove(remTeam);
        }

        protected void PlayerMessage(Player plr, string msg)
        {
            PlayerMessageEvent ev = new PlayerMessageEvent(plr, msg);
            AppendEvent(ev);
            if (OnPlayerMessage != null)
            {
                OnPlayerMessage(this, ev);
            }
        }

        protected void PlayerShipDestroyed(Player plr, Ship destroyedShip)
        {
            PlayerShipDestroyedEvent ev = new PlayerShipDestroyedEvent(plr, destroyedShip);
            AppendEvent(ev);
            if (OnPlayerShipDestruction != null)
            {
                OnPlayerShipDestruction(this, ev);
            }
        }

        private void AppendEvent(Event ev)
        {
            events.Add(ev);
            ev.Millis = (int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds - gameTimer.ElapsedMilliseconds);
        }

        private void ApplyParameters(Configuration conf)
        {
            fieldSize = new Coordinates(conf.GetValue<int>("mbc_field_width"), conf.GetValue<int>("mbc_field_height"));
            numberOfRounds = conf.GetValue<int>("mbc_match_rounds");

            startingShips = ShipList.ShipsFromLengths(conf.GetList<int>("mbc_ship_sizes"));
            timeLimit = conf.GetValue<int>("mbc_player_timeout");

            gameModes = conf.GetList<GameMode>("mbc_game_mode");
        }

        private void NotifyParamsChanged()
        {
            MatchConfigChangedEvent ev = new MatchConfigChangedEvent();
            AppendEvent(ev);
            if (OnConfigChange != null)
            {
                OnConfigChange(this, new MatchConfigChangedEvent());
            }
            OnConfigChange(this, new MatchConfigChangedEvent());
        }

        private void SetParameters(Configuration conf)
        {
            ApplyParameters(conf);
            NotifyParamsChanged();
        }
    }
}