using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;
using MBC.Shared.Events;
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
            GameTimer = new Stopwatch();
            CurrentRound = -1;

            ApplyParameters(config);
        }

        public int CurrentEvent
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the events that have been generated within the match.
        /// </summary>
        public List<Event> Events
        {
            get;
            protected set;
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        protected Stopwatch GameTimer
        {
            get;
            private set;
        }

        /// <summary>
        /// Adds a player to this match by instantiating a new controller through a specific
        /// controller skeleton.
        /// </summary>
        /// <param name="skeleton"></param>
        /// <returns></returns>
        public virtual bool AddPlayer(ControllerSkeleton skeleton)
        {
            return AddPlayer(new Player(FindFirstEmptyPlayerID(), skeleton.GetAttribute<NameAttribute>().Name, skeleton.CreateInstance()));
        }

        /// <summary>
        /// Assign a player to a team.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="newTeam"></param>
        public virtual bool AssignPlayerTeam(Player plr, Team newTeam)
        {
            return AppendEvent(new PlayerTeamAssignEvent(plr, newTeam));
        }

        /// <summary>
        /// Ends the match and prevents further progression.
        /// </summary>
        public bool EndMatch()
        {
            AtEnd = true;
            return AppendEvent(new MatchEndEvent(this));
        }

        /// <summary>
        /// Ends the round currently in progress.
        /// </summary>
        /// <returns></returns>
        public virtual bool EndRound()
        {
            return AppendEvent(new RoundEndEvent(this, CurrentRound));
        }

        /// <summary>
        /// Marks a player as a loser in the current round and increases their losing score.
        /// Player must be active in order to lose, otherwise this method returns false.
        /// </summary>
        /// <param name="plr"></param>
        public virtual bool MakePlayerLose(Player plr)
        {
            return AppendEvent(new PlayerLostEvent(plr));
        }

        /// <summary>
        /// Use when a player shoots.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="shot"></param>
        public virtual bool MakePlayerShot(Player plr, Shot shot)
        {
            return MakePlayerShot(plr, shot, null);
        }

        /// <summary>
        /// Use when a player shoots and hits a ship.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="shot"></param>
        /// <param name="shipHit"></param>
        /// <returns></returns>
        public virtual bool MakePlayerShot(Player plr, Shot shot, Ship shipHit)
        {
            return AppendEvent(new PlayerShotEvent(plr, shot, shipHit));
        }

        /// <summary>
        /// Call when a player has timed out.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="ex"></param>
        public virtual bool MakePlayerTimeout(Player plr, string method)
        {
            return AppendEvent(new PlayerTimeoutEvent(plr, method));
        }

        /// <summary>
        /// Call when a player has won a round.
        /// </summary>
        /// <param name="plr"></param>
        public virtual bool MakePlayerWin(Player plr)
        {
            return AppendEvent(new PlayerWonEvent(plr));
        }

        /// <summary>
        /// Ends the current round if not already ended, and begins a new round.
        /// </summary>
        /// <returns></returns>
        public virtual bool NewRound()
        {
            EndRound();
            return AppendEvent(new RoundBeginEvent(this, CurrentRound + 1));
        }

        /// <summary>
        /// Sets the ships that are assigned to a player.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="ships"></param>
        /// <returns></returns>
        public virtual bool PlacePlayerShips(Player plr, IList<Ship> ships)
        {
            return AppendEvent(new PlayerShipsPlacedEvent(plr, ships));
        }

        /// <summary>
        /// Moves the match progress forward at a standard pace.
        /// </summary>
        public virtual void Play()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                if (CurrentRound == -1)
                {
                    AppendEvent(new RoundBeginEvent(this, CurrentRound + 1));
                }
                while (IsRunning && !AtEnd)
                {
                    var isRoundPlaying = PlayLogic();
                    if ((!isRoundPlaying) && (NumberOfRounds - CurrentRound == 0))
                    {
                        EndMatch();
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

        public bool PlayToEvent(int eventIdx)
        {
            if (eventIdx < 0 || eventIdx >= Events.Count)
            {
                return false;
            }
            if (eventIdx < CurrentEvent)
            {
                for (; CurrentEvent >= eventIdx; CurrentEvent--)
                {
                    if (!Events[CurrentEvent].ApplyBackward())
                    {
                        return false;
                    }
                    NotifyEvent(Events[CurrentEvent]);
                    if (CurrentEvent == eventIdx)
                    {
                        break;
                    }
                }
            }
            else if (eventIdx > CurrentEvent)
            {
                for (; CurrentEvent <= eventIdx; CurrentEvent++)
                {
                    if (!Events[CurrentEvent].ApplyForward())
                    {
                        return false;
                    }
                    NotifyEvent(Events[CurrentEvent]);
                    if (CurrentEvent == eventIdx)
                    {
                        break;
                    }
                }
            }
            return true;
        }

        public bool PlayToLastEvent()
        {
            return PlayToEvent(Events.Count - 1);
        }

        /// <summary>
        /// Call when a player is to be removed from the match.
        /// </summary>
        /// <param name="plr"></param>
        public virtual bool RemovePlayer(Player plr)
        {
            return AppendEvent(new MatchRemovePlayerEvent(this, plr));
        }

        /// <summary>
        /// Call when a team is to be removed from the match.
        /// </summary>
        /// <param name="remTeam"></param>
        public virtual bool RemoveTeam(Team remTeam)
        {
            return AppendEvent(new MatchTeamRemoveEvent(this, remTeam));
        }

        /// <summary>
        /// Stops progression of the match.
        /// </summary>
        public virtual void Stop()
        {
            IsRunning = false;
        }

        /// <summary>
        /// Adds a player to the match.
        /// </summary>
        /// <param name="playerAdd"></param>
        /// <returns></returns>
        protected virtual bool AddPlayer(Player playerAdd)
        {
            return AppendEvent(new MatchAddPlayerEvent(this, playerAdd));
        }

        /// <summary>
        /// Adds a team to the match.
        /// </summary>
        /// <param name="newTeam"></param>
        /// <returns></returns>
        protected virtual bool AddTeam(Team addTeam)
        {
            return AppendEvent(new MatchTeamAddEvent(this, addTeam));
        }

        /// <summary>
        /// Signals a disqualification event against a player for a specific reason.
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        protected virtual bool PlayerDisqualified(Player plr, string reason)
        {
            return AppendEvent(new PlayerDisqualifiedEvent(plr, reason));
        }

        protected virtual bool PlayLogic()
        {
            return false;
        }

        private bool AppendEvent(Event ev)
        {
            if (PlayToLastEvent() && ev.ApplyForward())
            {
                Events.Add(ev);
                ev.Millis = (int)(GameTimer.ElapsedMilliseconds);
                return true;
            }
            return false;
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
        }

        /// <summary>
        /// Finds the first IDNumber that is unoccupied in the players list.
        /// </summary>
        /// <returns></returns>
        private int FindFirstEmptyPlayerID()
        {
            var numberSet = new HashSet<int>();
            foreach (var plr in Players)
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