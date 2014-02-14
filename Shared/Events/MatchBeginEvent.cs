using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="Match"/> that has begun.
    /// </summary>
    public class MatchBeginEvent : MatchEvent
    {
        /// <summary>
        /// Constructs this event
        /// </summary>
        public MatchBeginEvent(Match copyParams)
            : base(copyParams)
        {
            CurrentRound = copyParams.CurrentRound;
            FieldSize = copyParams.FieldSize;
            NumberOfRounds = copyParams.NumberOfRounds;
            Random = copyParams.Random;
            RoundMode = copyParams.RoundMode;
            StartingShips = copyParams.StartingShips;
            TimeLimit = copyParams.TimeLimit;
        }

        /// <summary>
        /// Gets the current round in progress in the match.
        /// </summary>
        public int CurrentRound
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the field size parameter of the match.
        /// </summary>
        public Coordinates FieldSize
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the number of rounds to satisfy in the match.
        /// </summary>
        public int NumberOfRounds
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the random number generator available to the match.
        /// </summary>
        public Random Random
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the round mode for the match.
        /// </summary>
        public RoundMode RoundMode
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets a generated list of ships that define the ships that are to be placed at the
        /// start of a round.
        /// </summary>
        public HashSet<Ship> StartingShips
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the time limit allowed for each decision in the match.
        /// </summary>
        public int TimeLimit
        {
            get;
            protected set;
        }

        public override bool ApplyBackward()
        {
            return ApplyForward();
        }

        public override bool ApplyForward()
        {
            Match.CurrentRound = CurrentRound;
            Match.FieldSize = FieldSize;
            Match.NumberOfRounds = NumberOfRounds;
            Match.Random = Random;
            Match.RoundMode = RoundMode;
            Match.StartingShips = StartingShips;
            Match.TimeLimit = TimeLimit;
            return true;
        }
    }
}