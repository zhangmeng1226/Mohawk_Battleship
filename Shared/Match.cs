using MBC.Core.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MBC.Shared
{
    public delegate void MatchAddPlayerHandler(Match match, MatchAddPlayerEvent ev);

    public delegate void MatchBeginHandler(Match match, MatchBeginEvent ev);

    public delegate void MatchEndHandler(Match match, MatchEndEvent ev);

    public delegate void MatchParamChangeHandler(Match match, MatchConfigChangedEvent ev);

    public delegate void MatchRemovePlayerHandler(Match match, MatchRemovePlayerEvent ev);

    public delegate void PlayerDisqualifiedHandler(Match match, PlayerDisqualifiedEvent ev);

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

    public delegate void TeamAddHandler(Match match, MatchTeamAddEvent ev);

    public delegate void TeamRemoveHandler(Match match, MatchTeamRemoveEvent ev);

    /// <summary>
    /// This is the new framework part of a match.
    /// </summary>
    public class Match
    {
        /// <summary>
        /// Called when the match begins. Called before any rounds begin.
        /// </summary>
        public event EventHandler<MatchBeginEvent> OnMatchBegin;

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
        /// Called when a player is disqualified for some reason.
        /// </summary>
        public event PlayerDisqualifiedHandler OnPlayerDisqualified;

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
        /// Gets a boolean value indicating whether or not the match is at the end and cannot
        /// progress further.
        /// </summary>
        public bool AtEnd
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the current round in progress in the match.
        /// </summary>
        public int CurrentRound
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the events that have been generated within the match.
        /// </summary>
        public List<Event> Events
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the field size parameter of the match.
        /// </summary>
        public Coordinates FieldSize
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the number of rounds to satisfy in the match.
        /// </summary>
        public int NumberOfRounds
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the players involved with the match.
        /// </summary>
        public HashSet<Player> Players
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the random number generator available to the match.
        /// </summary>
        public Random Random
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the round mode for the match.
        /// </summary>
        public RoundMode RoundMode
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets a generated list of ships that define the ships that are to be placed at the
        /// start of a round.
        /// </summary>
        public HashSet<Ship> StartingShips
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the teams that exist within the match.
        /// </summary>
        public HashSet<Team> Teams
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the time limit allowed for each decision in the match.
        /// </summary>
        public int TimeLimit
        {
            get;
            protected set;
        }

        /// <summary>
        /// Adds a player to the match.
        /// </summary>
        /// <param name="playerAdd"></param>
        /// <returns></returns>
        public virtual bool AddPlayer(Player playerAdd)
        {
            if (players.Add(playerAdd))
            {
                var ev = new MatchAddPlayerEvent(playerAdd);
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
        public virtual bool AddTeam(Team addTeam)
        {
            if (teams.Add(addTeam))
            {
                var ev = new MatchTeamAddEvent(addTeam);
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
        /// Signals a disqualification event against a player for a specific reason.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public virtual bool PlayerDisqualified(Player plr, string reason)
        {
            if (players.Contains(plr))
            {
                plr.Disqualifications++;
                var ev = new PlayerDisqualifiedEvent(plr, reason);
                AppendEvent(ev);
                if (OnPlayerDisqualified != null)
                {
                    OnPlayerDisqualified(this, ev);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Marks a player as a loser in the current round and increases their losing score.
        /// Player must be active in order to lose, otherwise this method returns false.
        /// </summary>
        /// <param name="plr"></param>
        public virtual bool PlayerLost(Player plr)
        {
            if (plr.Active)
            {
                plr.Losses++;
                var ev = new PlayerLostEvent(plr);
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
        /// Sets the ships that are assigned to a player.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="ships"></param>
        /// <returns></returns>
        public virtual bool PlayerSetShips(Player plr, IList<Ship> ships)
        {
            if (players.Contains(plr))
            {
                plr.Ships = ships;
                var ev = new PlayerShipsPlacedEvent(plr, ships);
                AppendEvent(ev);
                if (OnPlayerShipsPlaced != null)
                {
                    OnPlayerShipsPlaced(this, ev);
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
        public virtual bool PlayerShot(Player plr, Shot shot)
        {
            return PlayerShot(plr, shot, null);
        }

        /// <summary>
        /// Use when a player shoots and hits a ship.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="shot"></param>
        /// <param name="shipHit"></param>
        /// <returns></returns>
        public virtual bool PlayerShot(Player plr, Shot shot, Ship shipHit)
        {
            if (!players.Contains(plr) && players.Contains(shot.ReceiverPlr))
            {
                var ev = new PlayerShotEvent(plr, shot, shipHit);
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
        public virtual bool PlayerTeamAssign(Player plr, Team newTeam)
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
        public virtual bool PlayerTimeout(Player plr, string method)
        {
            if (players.Contains(plr))
            {
                var ev = new PlayerTimeoutEvent(plr, method);
                AppendEvent(ev);
                if (OnPlayerTimeout != null)
                {
                    OnPlayerTimeout(this, ev);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Call when a player has won a round.
        /// </summary>
        /// <param name="plr"></param>
        public virtual bool PlayerWin(Player plr)
        {
            if (players.Contains(plr))
            {
                plr.Wins++;
                var ev = new PlayerWonEvent(plr);
                AppendEvent(ev);
                if (OnPlayerWin != null)
                {
                    OnPlayerWin(this, ev);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Call when a player is to be removed from the match.
        /// </summary>
        /// <param name="plr"></param>
        public virtual bool RemovePlayer(Player plr)
        {
            if (players.Remove(plr))
            {
                var ev = new MatchRemovePlayerEvent(plr);
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
        public virtual bool RemoveTeam(Team remTeam)
        {
            if (teams.Remove(remTeam))
            {
                var ev = new MatchTeamRemoveEvent(remTeam);
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
        /// Checks if a player's ship are placed, and are valid within the parameters of the match.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool ShipsValid(Player player)
        {
            return player.Ships != null &&
                ShipList.AreEquivalentLengths(player.Ships, StartingShips) &&
                ShipList.AreShipsValid(player.Ships, FieldSize) &&
                ShipList.GetConflictingShips(player.Ships).Count() == 0;
        }

        /// <summary>
        /// Notifies the end of a round.
        /// </summary>
        protected bool EndRound()
        {
            //TODO: Check for invalid round ending.
            if (true)
            {
                RoundEndEvent ev = new RoundEndEvent(CurrentRound);
                AppendEvent(ev);
                if (OnRoundEnd != null)
                {
                    OnRoundEnd(this, ev);
                }
                return true;
            }
            return false;
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
        /// Called when the match ends.
        /// </summary>
        /// <returns></returns>
        protected bool MatchEnd()
        {
            if (!AtEnd)
            {
                AtEnd = true;
                var ev = new MatchEndEvent();
                AppendEvent(ev);
                if (OnMatchEnd != null)
                {
                    OnMatchEnd(this, ev);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Notifies a new round beginning.
        /// </summary>
        protected bool NewRound()
        {
            //TODO: Check for invalid round begin.
            if (true)
            {
                var ev = new RoundBeginEvent(CurrentRound);
                AppendEvent(ev);
                if (OnRoundBegin != null)
                {
                    OnRoundBegin(this, ev);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Call when a player is outputting a message.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="msg"></param>
        protected virtual bool PlayerMessage(Player plr, string msg)
        {
            if (players.Contains(plr))
            {
                var ev = new PlayerMessageEvent(plr, msg);
                AppendEvent(ev);
                if (OnPlayerMessage != null)
                {
                    OnPlayerMessage(this, ev);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Call when a ship from a player has been destroyed.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="destroyedShip"></param>
        protected virtual bool PlayerShipDestroyed(Player plr, Ship destroyedShip)
        {
            if (players.Contains(plr))
            {
                var ev = new PlayerShipDestroyedEvent(plr, destroyedShip);
                AppendEvent(ev);
                if (OnPlayerShipDestruction != null)
                {
                    OnPlayerShipDestruction(this, ev);
                }
                return true;
            }
            return false;
        }
    }
}