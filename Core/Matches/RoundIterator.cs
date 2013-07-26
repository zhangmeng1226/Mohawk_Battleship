using MBC.Core.Rounds;
using MBC.Core.Util;
using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MBC.Core.Matches
{
    /// <summary>
    /// Contains a collection of <see cref="Round"/>s for a <see cref="Match"/>. Should not be used
    /// in application code.
    /// </summary>
    [Configuration("mbc_match_rounds_mode", Mode.AllRounds,
        Description = "Determines the ending behaviour of a match based on a given number of rounds.",
        DisplayName = "Match Rounds Mode")]
    [Configuration("mbc_match_rounds", 100)]
    public abstract class RoundIterator
    {
        /// <summary>
        /// Indicates that the target number of <see cref="Round"/>s has been reached.
        /// </summary>
        [XmlIgnore]
        protected bool roundsReached;

        /// <summary>
        /// Initializes various internal variables.
        /// </summary>
        /// <param name="match">The <see cref="Match"/> to base settings on.</param>
        public RoundIterator(Match match)
        {
            MonitoringMatch = match;
            TargetRounds = MonitoringMatch.Config.GetValue<int>("mbc_match_rounds");
            CurrentRoundIdx = 0;
            RoundList = new List<Round>();
            roundsReached = false;
        }

        /// <summary>
        /// Default constructor does nothing.
        /// </summary>
        private RoundIterator()
        {
        }

        /// <summary>
        /// Provides various behaviours as to how a Match will handle the number of rounds it is configured
        /// with.
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Creates and plays through <see cref="Round"/>s until the number of <see cref="Round"/>s generated
            /// is equal to the number of rounds configured.
            /// </summary>
            /// <seealso cref="Round"/>
            AllRounds,

            /// <summary>
            /// Creates and plays through <see cref="Round"/>s until a <see cref="ControllerRegister"/> has
            /// reached the number of rounds configured.
            /// </summary>
            /// <seealso cref="Round"/>
            /// <seealso cref="ControllerRegister"/>
            FirstTo
        }

        /// <summary>
        /// Gets the current focused <see cref="Round"/>
        /// </summary>
        [XmlIgnore]
        public Round CurrentRound
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the index of the internal array of the current <see cref="Round"/> in view.
        /// </summary>
        [XmlIgnore]
        public int CurrentRoundIdx
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="RoundIterator.Mode"/> that is being used.
        /// </summary>
        public Mode PlayMode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of <see cref="Round"/>s stored.
        /// </summary>
        public List<Round> RoundList
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value that indicates the <see cref="RoundIterator.CurrentRound"/> is the final <see cref="Round"/>.
        /// </summary>
        [XmlIgnore]
        public bool TargetReached
        {
            get
            {
                return roundsReached & CurrentRound.Ended;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates the target number of <see cref="Round"/>s.
        /// </summary>
        public int TargetRounds
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the <see cref="Match"/> that is associated.
        /// </summary>
        [XmlIgnore]
        protected Match MonitoringMatch
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a <see cref="RoundIterator"/> for a specific <paramref name="match"/>.
        /// </summary>
        /// <param name="match">The <see cref="Match"/> that requires a new <see cref="RoundIterator"/>.</param>
        /// <returns>A <see cref="RoundIterator"/>.</returns>
        public static RoundIterator CreateRoundIteratorFor(Match match)
        {
            RoundIterator iterator = null;
            switch (match.Config.GetValue<Mode>("mbc_match_rounds_mode"))
            {
                case Mode.AllRounds:
                    iterator = new PlayAllRoundIterator(match);
                    break;

                case Mode.FirstTo:
                    iterator = new FirstToRoundIterator(match);
                    break;
            }
            return iterator;
        }

        /// <summary>
        /// Moves the <see cref="RoundIterator.CurrentRound"/> forward if the <see cref="RoundIterator.TargetReached"/>
        /// and returns a value indicating whether or not the <see cref="RoundIterator.CurrentRound"/> could move forward.
        /// </summary>
        /// <returns>A value indicating whether or not the <see cref="RoundIterator.CurrentRound"/> could move forward.</returns>
        internal bool NextRound()
        {
            if (CurrentRound == null)
            {
                CurrentRound = MonitoringMatch.CreateNewRound();
                CurrentRound.Event += MonitoringMatch.RoundEventGenerated;
                return false;
            }
            else if (CurrentRoundIdx + 1 < RoundList.Count)
            {
                CurrentRound = RoundList[++CurrentRoundIdx];
                return false;
            }
            else if (!TargetReached)
            {
                Round newRound = MonitoringMatch.CreateNewRound();
                newRound.Event += MonitoringMatch.RoundEventGenerated;
                RoundList.Add(newRound);
                CurrentRound = newRound;
                CurrentRoundIdx++;
                roundsReached = IsRoundsReached();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Moves the <see cref="RoundIterator.CurrentRound"/> backward if it is not the first <see cref="Round"/>
        /// that had been generated.
        /// </summary>
        /// <returns>A value indicating whether or not the <see cref="RoundIterator.CurrentRound"/> is the first
        /// <see cref="Round"/>.</returns>
        internal bool PrevRound()
        {
            if (CurrentRoundIdx > 0)
            {
                CurrentRound = RoundList[--CurrentRoundIdx];
                return false;
            }
            return true;
        }

        /// <summary>
        /// Abstract method used to determine whether or not the <see cref="RoundIterator.TargetReached"/>
        /// based on the <see cref="RoundIterator.Mode"/>.
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsRoundsReached();

        /// <summary>
        /// Determines if the <see cref="RoundIterator.TargetRounds"/> has been attained by
        /// a single <see cref="Register"/>.
        /// </summary>
        private class FirstToRoundIterator : RoundIterator
        {
            public FirstToRoundIterator(Match match)
                : base(match)
            {
            }

            private FirstToRoundIterator() { }

            protected override bool IsRoundsReached()
            {
                foreach (var register in MonitoringMatch.Registers)
                {
                    if (register.Score >= TargetRounds)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Determines if the <see cref="RoundIterator.TargetRounds"/> has been met
        /// with the current number of <see cref="Round"/>s in the <see cref="RoundIterator"/> collection.
        /// </summary>
        private class PlayAllRoundIterator : RoundIterator
        {
            public PlayAllRoundIterator(Match match)
                : base(match)
            {
            }

            private PlayAllRoundIterator() { }

            protected override bool IsRoundsReached()
            {
                return CurrentRoundIdx + 1 >= TargetRounds;
            }
        }
    }
}