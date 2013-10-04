using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MBC.Shared
{
    public class MatchConfig : ISerializable
    {
        public MatchConfig()
        {
        }

        public MatchConfig(MatchConfig copy)
        {
            StartingShips = new ShipList(copy.StartingShips);
            Registers = new Dictionary<IDNumber, Register>();
            foreach (var regPair in copy.Registers)
            {
                Registers[regPair.Key] = new Register(regPair.Value);
            }
            FieldSize = copy.FieldSize;
            TimeLimit = copy.TimeLimit;
            GameMode = copy.GameMode;
        }

        private MatchConfig(SerializationInfo info, StreamingContext context)
        {
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
        /// Gets the <see cref="GameMode"/> that determines the behaviour of the running rounds in the match.
        /// </summary>
        public GameMode GameMode
        {
            get;
            set;
        }

        public int NumberOfRounds
        {
            get;
            set;
        }

        public RoundMode RoundMode
        {
            get;
            set;
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
        /// Gets the time in milliseconds of the max allowed time of a <see cref="Controller"/> to return from a method invoke.
        /// </summary>
        public int TimeLimit
        {
            get;
            set;
        }

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //Do not serialize teams/registers
        }
    }
}