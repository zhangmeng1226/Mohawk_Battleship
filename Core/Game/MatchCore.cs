using MBC.Core.Events;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace MBC.Core.Game
{
    /// <summary>
    /// This is the new framework part of a match.
    /// </summary>
    public class MatchCore : Match
    {
        protected Stopwatch gameTimer;

        /// <summary>
        /// Creates a match with parameters loaded from a configuration.
        /// </summary>
        /// <param name="config"></param>
        public MatchCore(Configuration config)
        {
            Events = new List<Event>();
            Players = new HashSet<Player>();
            Teams = new HashSet<Team>();
            Random = new Random();
            gameTimer = new Stopwatch();

            ApplyParameters(config);
        }

        public bool IsRunning
        {
            get;
            private set;
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
        /// Moves the match progress forward at a standard pace.
        /// </summary>
        public virtual void Play()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                while (IsRunning && !AtEnd)
                {
                    var isRoundPlaying = PlayLogic();
                    if ((!isRoundPlaying) && (NumberOfRounds - CurrentRound == 0))
                    {
                        MatchEnd();
                        break;
                    }
                    else if (!isRoundPlaying)
                    {
                        EndRound();
                        CurrentRound++;
                        NewRound();
                    }
                }
            }
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
            if (Teams.Remove(remTeam))
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
        /// Stops progression of the match.
        /// </summary>
        public virtual void Stop()
        {
            IsRunning = false;
        }

        protected virtual bool PlayLogic()
        {
            return false;
        }

        /// <summary>
        /// Appends an event to the list of events of the match, with a proper timestamp.
        /// </summary>
        /// <param name="ev"></param>
        private void AppendEvent(Event ev)
        {
            Events.Add(ev);
            ev.Millis = (int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds - gameTimer.ElapsedMilliseconds);
        }

        /// <summary>
        /// Applies a configuration to the match.
        /// </summary>
        /// <param name="conf"></param>
        private void ApplyParameters(Configuration conf)
        {
            FieldSize = new Coordinates(conf.GetValue<int>("mbc_field_width"), conf.GetValue<int>("mbc_field_height"));
            NumberOfRounds = conf.GetValue<int>("mbc_match_rounds");

            StartingShips = ShipList.ShipsFromLengths(conf.GetList<int>("mbc_ship_sizes"));
            TimeLimit = conf.GetValue<int>("mbc_player_timeout");

            GameModes = conf.GetList<GameMode>("mbc_game_mode");
        }

        /// <summary>
        /// Finds the first IDNumber that is unoccupied in the players list.
        /// </summary>
        /// <returns></returns>
        private int FindFirstEmptyPlayerID()
        {
            var numberSet = new HashSet<int>();
            foreach (var plr in players)
            {
                numberSet.Add(plr.ID);
            }
            for (int i = 0; i < numberSet.Count; i++)
            {
                if (numberSet.Contains(i))
                {
                    return i;
                }
            }
            return numberSet.Count;
        }
    }
}