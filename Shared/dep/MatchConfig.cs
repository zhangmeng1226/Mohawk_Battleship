using MBC.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MBC.Shared
{
    [Obsolete("Match parameters located within the match object.")]
    public class MatchConfig : ISerializable
    {
        private ShipList startingShips;

        public MatchConfig()
        {
        }

        public MatchConfig(MatchConfig copy)
        {
            StartingShips = new ShipList(copy.StartingShips);
            NumberOfRounds = copy.NumberOfRounds;
            FieldSize = copy.FieldSize;
            TimeLimit = copy.TimeLimit;
            GameMode = copy.GameMode;
            Random = copy.Random;
        }

        public MatchConfig(Match matchCopy)
        {
            if (matchCopy.StartingShips != null)
            {
                StartingShips = new ShipList(matchCopy.StartingShips.ToList());
            }
            NumberOfRounds = matchCopy.NumberOfRounds;
            FieldSize = matchCopy.FieldSize;
            TimeLimit = matchCopy.TimeLimit;
            GameMode = GameMode.Classic;
            Random = matchCopy.Random;
        }

        public MatchConfig(SerializationInfo info, StreamingContext context)
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

        public Random Random
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
            get
            {
                return new ShipList(startingShips);
            }
            set
            {
                startingShips = value;
            }
        }

        /// <summary>
        /// Gets the time in milliseconds of the max allowed time of a <see cref="Controller"/> to return from a method invoke.
        /// </summary>
        public int TimeLimit
        {
            get;
            set;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}