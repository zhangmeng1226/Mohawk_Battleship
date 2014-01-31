using MBC.Core.Events;
using MBC.Core.Threading;
using MBC.Core.Util;
using MBC.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

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

    public delegate void PlayerTurnSwitchHandler(Match match, PlayerTurnSwitchEvent ev);

    public delegate void PlayerWonHandler(Match match, PlayerWonEvent ev);

    public delegate void RoundBeginHandler(Match match, RoundBeginEvent ev);

    public delegate void RoundEndHandler(Match match, RoundEndEvent ev);

    public delegate void TeamAddHandler(Match match, MatchTeamCreateEvent ev);

    public delegate void TeamRemoveHandler(Match match, MatchTeamRemoveEvent ev);

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
        private int currentRound;
        private List<Event> events;
        private Coordinates fieldSize;
        private List<GameMode> gameModes;
        private Stopwatch gameTimer;
        private int numberOfRounds;
        private HashSet<Player> players;
        private Random randObj;
        private RoundMode rndBehavior;
        private List<Ship> startingShips;
        private HashSet<Team> teams;
        private int timeLimit;

        /// <summary>
        /// Creates a match with parameters loaded from a configuration.
        /// </summary>
        /// <param name="config"></param>
        public Match(Configuration config)
        {
            events = new List<Event>();
            players = new HashSet<Player>();
            teams = new HashSet<Team>();
            gameTimer = new Stopwatch();
            randObj = new Random();

            ApplyParameters(config);
        }

        /// <summary>
        /// Called when the match begins. Called before any rounds begin.
        /// </summary>
        public event MatchBeginHandler OnMatchBegin;

        /// <summary>
        /// Called when the match ends. Called after all rounds have ended and the
        /// match conditions have been met.
        /// </summary>
        public event MatchEndHandler OnMatchEnd;

        /// <summary>
        /// Called when the match parameters have changed.
        /// </summary>
        public event MatchParamChangeHandler OnParamChange;

        /// <summary>
        /// Called when a player is added to the match.
        /// </summary>
        public event MatchAddPlayerHandler OnPlayerAdd;

        /// <summary>
        /// Called when a players loses during the match.
        /// </summary>
        public event PlayerLostHandler OnPlayerLose;

        /// <summary>
        /// Called when a player outputs a message.
        /// </summary>
        public event PlayerMessageHandler OnPlayerMessage;

        /// <summary>
        /// Called when a player is removed from the match.
        /// </summary>
        public event MatchRemovePlayerHandler OnPlayerRemove;

        /// <summary>
        /// Called when a player's ship has been hit completely, and has been
        /// removed from the player's list of ships.
        /// </summary>
        public event PlayerShipDestroyedHandler OnPlayerShipDestruction;

        /// <summary>
        /// Called when a player finalizes their ship placement in the beginning of a round,
        /// or when the position of any of the ships move during a round.
        /// </summary>
        public event PlayerShipsPlacedHandler OnPlayerShipsPlaced;

        /// <summary>
        /// Called when a player makes a shot against an opponent.
        /// </summary>
        public event PlayerShotHandler OnPlayerShot;

        /// <summary>
        /// Called when a player is assigned to a team.
        /// </summary>
        public event PlayerTeamAssignHandler OnPlayerTeamAssign;

        /// <summary>
        /// Called when a player times out during their turn.
        /// </summary>
        public event PlayerTimeoutHandler OnPlayerTimeout;

        /// <summary>
        /// Called when a player turn switches to another player.
        /// </summary>
        public event PlayerTurnSwitchHandler OnPlayerTurnSwitch;

        /// <summary>
        /// Called when a player wins a round.
        /// </summary>
        public event PlayerWonHandler OnPlayerWin;

        /// <summary>
        /// Called when a round begins.
        /// </summary>
        public event RoundBeginHandler OnRoundBegin;

        /// <summary>
        /// Called when a round ends.
        /// </summary>
        public event RoundEndHandler OnRoundEnd;

        /// <summary>
        /// Called when a team has been added to the match.
        /// </summary>
        public event TeamAddHandler OnTeamAdd;

        /// <summary>
        /// Called when a team has been removed from the match.
        /// </summary>
        public event TeamRemoveHandler OnTeamRemove;

        /// <summary>
        /// Gets the current round in progress in the match.
        /// </summary>
        public int CurrentRound
        {
            get
            {
                return currentRound;
            }
        }

        /// <summary>
        /// Gets or sets the field size parameter of the match.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the game mode of the match.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the number of rounds to satisfy in the match.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the round mode for the match.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the time limit allowed for each decision in the match.
        /// </summary>
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

        /// <summary>
        /// Adds a player to the match.
        /// </summary>
        /// <param name="plr"></param>
        /// <returns></returns>
        public bool AddPlayer(Player plr)
        {
            if (players.Add(plr))
            {
                plr.PropertyChanged += PlayerPropertyChange;
                MatchAddPlayerEvent ev = new MatchAddPlayerEvent(plr);
                AppendEvent(ev);
                if (OnPlayerAdd != null)
                {
                    OnPlayerAdd(this, ev);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a team to the match.
        /// </summary>
        /// <param name="newTeam"></param>
        /// <returns></returns>
        public bool AddTeam(Team newTeam)
        {
            if (teams.Add(newTeam))
            {
                MatchTeamCreateEvent ev = new MatchTeamCreateEvent(newTeam);
                AppendEvent(ev);
                if (OnTeamAdd != null)
                {
                    OnTeamAdd(this, ev);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Moves the match progress forward at a standard pace.
        /// </summary>
        public virtual void Play()
        {
        }

        /// <summary>
        /// Marks a player as a loser in the current round and increases their losing score.
        /// Player must be active in order to lose, otherwise this method returns false.
        /// </summary>
        /// <param name="plr"></param>
        public bool PlayerLost(Player plr)
        {
            if (plr.Active)
            {
                plr.Losses++;
                PlayerLostEvent ev = new PlayerLostEvent(plr);
                AppendEvent(ev);
                if (OnPlayerLose != null)
                {
                    OnPlayerLose(this, ev);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Use when a player shoots.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="shot"></param>
        public bool PlayerShot(Player plr, Shot shot)
        {
            if (IsShotValid(shot))
            {
                Ship shipHit = ShipList.GetShipAt(shot.ReceiverPlr.Ships, shot.Coordinates);
                PlayerShotEvent ev = new PlayerShotEvent(plr, shot, shipHit);
                AppendEvent(ev);
                if (OnPlayerShot != null)
                {
                    OnPlayerShot(this, ev);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Assign a player to a team.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="newTeam"></param>
        public bool PlayerTeamAssign(Player plr, Team newTeam)
        {
            if (plr.Team != newTeam)
            {
                plr.Team = newTeam;
                PlayerTeamAssignEvent ev = new PlayerTeamAssignEvent(plr, newTeam);
                AppendEvent(ev);
                if (OnPlayerTeamAssign != null)
                {
                    OnPlayerTeamAssign(this, ev);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Call when a player has timed out.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="ex"></param>
        public void PlayerTimeout(Player plr, ControllerTimeoutException ex)
        {
            PlayerTimeoutEvent ev = new PlayerTimeoutEvent(plr, ex);
            AppendEvent(ev);
            if (OnPlayerTimeout != null)
            {
                OnPlayerTimeout(this, ev);
            }
        }

        /// <summary>
        /// Call when a player has won a round.
        /// </summary>
        /// <param name="plr"></param>
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

        /// <summary>
        /// Call when a player is to be removed from the match.
        /// </summary>
        /// <param name="plr"></param>
        public bool RemovePlayer(Player plr)
        {
            if (players.Remove(plr))
            {
                MatchRemovePlayerEvent ev = new MatchRemovePlayerEvent(plr);
                AppendEvent(ev);
                if (OnPlayerRemove != null)
                {
                    OnPlayerRemove(this, ev);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Call when a team is to be removed from the match.
        /// </summary>
        /// <param name="remTeam"></param>
        public bool RemoveTeam(Team remTeam)
        {
            if (teams.Remove(remTeam))
            {
                MatchTeamRemoveEvent ev = new MatchTeamRemoveEvent(remTeam);
                AppendEvent(ev);
                if (OnTeamRemove != null)
                {
                    OnTeamRemove(this, ev);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Stops progression of the match.
        /// </summary>
        public virtual void Stop()
        {
        }

        /// <summary>
        /// Determines whether or not a given shot is valid within the parameters of the match.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="shot"></param>
        /// <returns></returns>
        protected virtual bool IsShotValid(Shot shot)
        {
            return players.Contains(shot.ReceiverPlr) ||
                (shot.Coordinates < fieldSize && shot.Coordinates >= new Coordinates(0, 0));
        }

        /// <summary>
        /// Call when a player is outputting a message.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="msg"></param>
        protected void PlayerMessage(Player plr, string msg)
        {
            PlayerMessageEvent ev = new PlayerMessageEvent(plr, msg);
            AppendEvent(ev);
            if (OnPlayerMessage != null)
            {
                OnPlayerMessage(this, ev);
            }
        }

        /// <summary>
        /// Call when a ship from a player has been destroyed.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="destroyedShip"></param>
        protected void PlayerShipDestroyed(Player plr, Ship destroyedShip)
        {
            PlayerShipDestroyedEvent ev = new PlayerShipDestroyedEvent(plr, destroyedShip);
            AppendEvent(ev);
            if (OnPlayerShipDestruction != null)
            {
                OnPlayerShipDestruction(this, ev);
            }
        }

        /// <summary>
        /// Changes the match parameters to reflect the configuration given.
        /// </summary>
        /// <param name="conf"></param>
        protected void SetParameters(Configuration conf)
        {
            ApplyParameters(conf);
            NotifyParamsChanged();
        }

        /// <summary>
        /// Appends an event to the list of events of the match, with a proper timestamp.
        /// </summary>
        /// <param name="ev"></param>
        private void AppendEvent(Event ev)
        {
            events.Add(ev);
            ev.Millis = (int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds - gameTimer.ElapsedMilliseconds);
        }

        /// <summary>
        /// Applies a configuration to the match.
        /// </summary>
        /// <param name="conf"></param>
        private void ApplyParameters(Configuration conf)
        {
            fieldSize = new Coordinates(conf.GetValue<int>("mbc_field_width"), conf.GetValue<int>("mbc_field_height"));
            numberOfRounds = conf.GetValue<int>("mbc_match_rounds");

            startingShips = ShipList.ShipsFromLengths(conf.GetList<int>("mbc_ship_sizes"));
            timeLimit = conf.GetValue<int>("mbc_player_timeout");

            gameModes = conf.GetList<GameMode>("mbc_game_mode");
        }

        /// <summary>
        /// Notifies the end of a round.
        /// </summary>
        private void EndRound()
        {
            RoundEndEvent ev = new RoundEndEvent(CurrentRound);
            AppendEvent(ev);
            if (OnRoundEnd != null)
            {
                OnRoundEnd(this, ev);
            }
        }

        /// <summary>
        /// Notifies a new round beginning.
        /// </summary>
        private void NewRound()
        {
            RoundBeginEvent ev = new RoundBeginEvent(CurrentRound);
            AppendEvent(ev);
            if (OnRoundBegin != null)
            {
                OnRoundBegin(this, ev);
            }
        }

        /// <summary>
        /// Notifies match parameter changes.
        /// </summary>
        private void NotifyParamsChanged()
        {
            MatchConfigChangedEvent ev = new MatchConfigChangedEvent();
            AppendEvent(ev);
            if (OnParamChange != null)
            {
                OnParamChange(this, new MatchConfigChangedEvent());
            }
            OnParamChange(this, new MatchConfigChangedEvent());
        }

        /// <summary>
        /// Called when a player in the match has had a property changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            var player = sender as Player;
            switch (e.PropertyName)
            {
                case Player.PROPERTY_TEAM:
                    break;
            }
        }
    }
}