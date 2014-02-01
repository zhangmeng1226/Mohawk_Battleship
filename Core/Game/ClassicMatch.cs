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
        /// An enum that
        /// </summary>
        public enum Phase
        {
            Placement,
            Turn
        }

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
        ///
        /// </summary>
        protected override void PlayLogic()
        {
        }
    }
}