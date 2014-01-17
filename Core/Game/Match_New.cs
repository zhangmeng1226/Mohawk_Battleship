using MBC.Core.Events;
using MBC.Core.Util;
using MBC.Shared;
using System;
using System.Collections.Generic;
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
        private List<Event> events;
        private List<Player> players;
        private List<Team> teams;

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
    }
}