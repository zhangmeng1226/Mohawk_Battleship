using MBC.Core.Util;
using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Game
{
    /// <summary>
    /// A type of match that uses the standard rules of battleship as logic.
    /// </summary>
    public class ClassicMatch : Match
    {
        /// <summary>
        /// Constructs a match with a specific configuration.
        /// </summary>
        /// <param name="conf"></param>
        public ClassicMatch(Configuration conf)
            : base(conf)
        {
            CurrentPhase = Phase.Init;
        }

        /// <summary>
        /// Constructs a match with the application-wide configuration.
        /// </summary>
        public ClassicMatch()
            : this(Configuration.Global)
        {
        }

        /// <summary>
        /// Defines all of the states that are possible within the match.
        /// </summary>
        public enum Phase
        {
            Init,
            Placement,
            Turn
        }

        /// <summary>
        /// Gets the current phase of this type of match.
        /// </summary>
        public Phase CurrentPhase
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the player that has the turn.
        /// </summary>
        public Player CurrentPlayer
        {
            get;
            protected set;
        }

        /// <summary>
        /// Plays through the current state of the match, in one iteration.
        /// </summary>
        protected override void PlayLogic()
        {
            switch (CurrentPhase)
            {
                case Phase.Init:
                    Initialization();
                    break;

                case Phase.Placement:
                    Placement();
                    break;

                case Phase.Turn:
                    Turn();
                    break;
            }
        }

        private void Initialization()
        {
            foreach (Player plr in Players)
            {
                plr.Active = true;
                plr.Ships = StartingShips;
            }
            CurrentPhase = Phase.Placement;
        }

        private void Placement()
        {
            CurrentPhase = Phase.Turn;
        }

        private void Turn()
        {
        }
    }
}