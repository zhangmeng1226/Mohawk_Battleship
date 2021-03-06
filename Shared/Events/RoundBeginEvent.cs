﻿using MBC.Shared;
using MBC.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="GameLogic"/> that has begun.
    /// </summary>
    [Serializable]
    public class RoundBeginEvent : MatchEvent
    {
        /// <summary>
        /// Passes the <paramref name="round"/> to the base constructor
        /// based on the <see cref="MBC.Shared.Register"/>s that are involved in it.
        /// </summary>
        /// <param name="round">The associated <see cref="GameLogic"/>.</param>
        public RoundBeginEvent(Match match)
            : base(match)
        {
            Round = match.CurrentRound + 1;
        }

        /// <summary>
        /// Gets the round number.
        /// </summary>
        public IDNumber Round
        {
            get;
            private set;
        }

        protected internal override void PerformOperation()
        {
            if ((Match.CurrentRound + 1) != Round)
            {
                throw new InvalidEventException(this, "The round that is beginning is not correct.");
            }
            if (Match.Players.Count < 2)
            {
                throw new InvalidEventException(this, "The match must have at least two players playing for a round to run.");
            }

            Match.CurrentRound++;
            foreach (Player plr in Match.Players)
            {
                plr.Active = true;
                plr.ShotsMade.Clear();
                if (Match.CurrentRound > 0)
                {
                    foreach (Ship ship in plr.Ships)
                    {
                        ship.Reset();
                    }
                }
            }
            Match.TurnOrder = new List<Player>(Match.Players);
            RandomizeList(Match.TurnOrder);
            Match.CurrentPlayer = Match.TurnOrder.First();
        }

        /// <summary>
        /// Randomizes the order of a list of elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        private static void RandomizeList<T>(IList<T> list)
        {
            Random rand = new Random();
            for (int i = 0; i < list.Count; i++)
            {
                int randIdx = rand.Next(i + 1);
                T value = list[randIdx];
                list[randIdx] = list[i];
                list[i] = value;
            }
        }
    }
}