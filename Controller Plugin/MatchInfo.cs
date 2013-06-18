using System.Collections.Generic;

namespace MBC.Shared
{
    /// <summary>
    /// Provides information about a single match in a game of battleship. Remains consistent and un-modified
    /// throughout the match.
    /// </summary>
    public class MatchInfo
    {
        /// <summary>
        /// A list of strings of all <see cref="Controller"/>s involved. Respects the order of
        /// <see cref="ControllerRegister.ID"/> of each <see cref="Controller"/>.
        /// </summary>
        protected List<string> controllerNames;

        /// <summary>
        /// A <see cref="ShipList"/> containing the <see cref="Ship"/>s that a <see cref="Controller"/> is
        /// given at the beginning of a round for placement.
        /// </summary>
        protected ShipList initShips;

        /// <summary>
        /// <see cref="Coordinates"/> with the <see cref="Coordinates.X"/> and <see cref="Coordinates.Y"/>
        /// values that indicate the maximum size of the battleship field.
        /// <remarks>
        /// The battleship field starts at (0, 0) and controllers may place <see cref="Ship"/>s and
        /// <see cref="Shot"/>s less than the maximum size.
        /// </remarks>
        /// </summary>
        protected Coordinates fieldSize;

        /// <summary>
        /// The time in milliseconds that a <see cref="Controller"/> has to return from a method.
        /// </summary>
        protected int methodTimeLimit;

        /// <summary>
        /// The <see cref="GameMode"/> that a match is running in.
        /// </summary>
        protected GameMode gameMode;

        /// <summary>
        /// Does nothing, but makes it so only deriving classes may create a <see cref="MatchInfo"/>.
        /// </summary>
        protected MatchInfo()
        {
        }

        /// <summary>
        /// Gets a <see cref="ShipList"/> that contains the <see cref="Ship"/> objects that a controller 
        /// can place at the beginning of a round.
        /// </summary>
        public ShipList StartingShips
        {
            get
            {
                return new ShipList(initShips);
            }
        }

        /// <summary>
        /// Gets the <see cref="Coordinates"/> that identify the maximum size of the battleship field through
        /// the <see cref="Coordinates.X"/> (width) and <see cref="Coordinates.Y"/> (height) components.
        /// <remarks>
        /// The battleship field starts at (0, 0) and controllers may place <see cref="Ship"/>s and
        /// <see cref="Shot"/>s less than the maximum size.
        /// </remarks>
        /// </summary>
        public Coordinates FieldSize
        {
            get
            {
                return fieldSize;
            }
        }

        /// <summary>
        /// Gets the time in milliseconds of the max allowed time of a <see cref="Controller"/> to return from a method invoke.
        /// </summary>
        public int TimeLimit
        {
            get
            {
                return methodTimeLimit;
            }
        }

        /// <summary>
        /// Gets the <see cref="GameMode"/> that determines the behaviour of the running rounds in the match.
        /// </summary>
        public GameMode GameMode
        {
            get
            {
                return gameMode;
            }
        }

        /// <summary>
        /// Gets a list of strings of all <see cref="Controller"/>s involved. Respects the order of
        /// <see cref="ControllerRegister.ID"/> of each <see cref="Controller"/>.
        /// </summary>
        /// <seealso cref="ControllerID"/>
        public List<string> ControllerNames
        {
            get
            {
                return controllerNames;
            }
        }
    }
}