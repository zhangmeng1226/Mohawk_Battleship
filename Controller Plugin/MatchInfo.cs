using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core
{
    /// <summary>
    /// Provides information about a matchup. Each MatchInfo is persistent throughout each matchup.
    /// </summary>
    public class MatchInfo
    {
        protected List<string> playerNames;
        protected ShipList initShips;
        protected Coordinates fieldSize;
        protected int methodTimeLimit;
        protected GameMode gameMode;

        protected MatchInfo() { }

        /// <summary>
        /// Gets a ShipList that contains the Ship objects that a controller can place at the beginning of a
        /// Round.
        /// </summary>
        public ShipList StartingShips
        {
            get
            {
                return new ShipList(initShips);
            }
        }

        /// <summary>
        /// Gets the maximum X and Y range of the field that a controller can fire within.
        /// </summary>
        public Coordinates FieldSize
        {
            get
            {
                return fieldSize;
            }
        }
        /// <summary>
        /// Gets the time in milliseconds of the max allowed time of a controller to return from a method invoke.
        /// </summary>
        public int TimeLimit
        {
            get
            {
                return methodTimeLimit;
            }
        }

        /// <summary>
        /// Gets the GameMode that determines the behaviour of the running rounds.
        /// </summary>
        public GameMode GameMode
        {
            get
            {
                return gameMode;
            }
        }

        /// <summary>
        /// Gets a List of strings that names each of the controllers that are involved in the match.
        /// The order will be the same as Controllers are ordered in a Round, as can be accessed with a
        /// ControllerID.
        /// </summary>
        /// <seealso cref="ControllerID"/>
        public List<string> Players
        {
            get
            {
                return new List<string>(playerNames);
            }
        }
    }
}
