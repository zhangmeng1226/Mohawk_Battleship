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
        /// <summary>
        /// Creates a match with parameters loaded from a configuration.
        /// </summary>
        /// <param name="config"></param>
        public MatchCore(Configuration config)
        {
            events = new List<Event>();
            players = new HashSet<Player>();
            teams = new HashSet<Team>();
            gameTimer = new Stopwatch();
            randObj = new Random();

            ApplyParameters(config);
        }

        public bool IsRunning
        {
            get;
            private set;
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
                    if ((!isRoundPlaying) && (NumberOfRounds - currentRound == 0))
                    {
                        MatchEnd();
                        break;
                    }
                    else if (!isRoundPlaying)
                    {
                        EndRound();
                        currentRound++;
                        NewRound();
                    }
                }
            }
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