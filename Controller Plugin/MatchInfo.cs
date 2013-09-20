using System.Collections.Generic;

namespace MBC.Shared
{
    /// <summary>
    /// Provides information about a single match in a game of battleship. Remains consistent and un-modified
    /// throughout the match.
    /// </summary>
    public class MatchInfo
    {

        public MatchInfo()
        {

        }

        public MatchInfo(MatchInfo copy)
        {
            StartingShips = new ShipList(copy.StartingShips);
            Registers = new List<Register>();
            foreach (var register in copy.Registers)
            {
                Registers.Add(new Register(register));
            }
            FieldSize = copy.FieldSize;
            TimeLimit = copy.TimeLimit;
            GameMode = copy.GameMode;
        }

        /// <summary>
        /// Gets a <see cref="ShipList"/> that contains the <see cref="Ship"/> objects that a controller 
        /// can place at the beginning of a round.
        /// </summary>
        public ShipList StartingShips
        {
            get;
            set;
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
            get;
            set;
        }

        /// <summary>
        /// Gets the time in milliseconds of the max allowed time of a <see cref="Controller"/> to return from a method invoke.
        /// </summary>
        public int TimeLimit
        {
            get;
            set;
        }

        public int NumberOfRounds
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the <see cref="GameMode"/> that determines the behaviour of the running rounds in the match.
        /// </summary>
        public GameMode GameMode
        {
            get;
            set;
        }

        public RoundMode RoundMode
        {
            get;
            set;
        }

        public List<Register> Registers
        {
            get;
            set;
        }
    }
}