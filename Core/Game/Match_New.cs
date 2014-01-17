using MBC.Core.Events;
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

    public delegate void PlayerHitShipHandler(Match match, PlayerHitShipEvent ev);

    public delegate void PlayerLostHandler(Match match, PlayerLostEvent ev);

    public delegate void PlayerMessageHandler(Match match, PlayerMessageEvent ev);

    public delegate void PlayerShipDestroyedHandler(Match match, PlayerShipDestroyedEvent ev);

    public delegate void PlayerShipsPlacedHandler(Match match, PlayerShipsPlacedEvent ev);

    public delegate void PlayerShotHandler(Match match, PlayerShotEvent ev);

    public delegate void PlayerTeamAssignHandler(Match match, PlayerTeamAssignEvent ev);

    public delegate void PlayerTimeoutHandler(Match match, PlayerTimeoutEvent ev);

    public delegate void PlayerWonHandler(Match match, PlayerWonEvent ev);

    public delegate void RemovePlayerHandler(Match match, RemovePlayerHandler ev);

    public delegate void RoundBeginHandler(Match match, RoundBeginEvent ev);

    public delegate void RoundEndHandler(Match match, RoundEndEvent ev);

    public delegate void TeamAddHandler(Match match, TeamAddHandler ev);

    public delegate void TeamRemoveHandler(Match match, TeamRemoveHandler ev);

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
        private Coordinates boardSize;
        private List<Event> events;
        private GameMode gameMode;
        private Stopwatch gameTimer;
        private int numberOfRounds;
        private List<Player> players;
        private Random randObj;
        private RoundMode rndBehavior;
        private List<Ship> startingShips;
        private List<Team> teams;
        private int timeLimit;

        public Match(bool doIt)
        {
        }

        public event MatchParamChangeHandler OnConfigChange;

        public event MatchBeginHandler OnMatchBegin;

        public event MatchEndHandler OnMatchEnd;

        public event MatchAddPlayerHandler OnPlayerAdd;

        public event PlayerHitShipHandler OnPlayerHitShip;

        public event PlayerLostHandler OnPlayerLose;

        public event PlayerMessageHandler OnPlayerMessage;

        public event RemovePlayerHandler OnPlayerRemove;

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
                return boardSize;
            }
            set
            {
                boardSize = value;
                MatchConfigChangedEvent ev = new MatchConfigChangedEvent();
                AppendEvent(ev);
                if (OnConfigChange != null)
                {
                    OnConfigChange(this, );
                }
                OnConfigChange(this, new MatchConfigChangedEvent());
            }
        }

        public void SetParameters(Configuration conf)
        {
        }

        private void AppendEvent(Event ev)
        {
            events.Add(ev);
            ev.Millis = (int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds - evTime);
        }

        private void NotifyParamsChanged()
        {
        }
    }
}