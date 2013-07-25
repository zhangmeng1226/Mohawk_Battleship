using MBC.Core.Rounds;
using MBC.Core.Util;
using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MBC.Core.Matches
{
    [Configuration("mbc_match_rounds_mode", Mode.AllRounds,
        Description = "Determines the ending behaviour of a match based on a given number of rounds.",
        DisplayName = "Match Rounds Mode")]
    [Configuration("mbc_match_rounds", 100)]
    public abstract class RoundIterator
    {
        [XmlIgnore]
        protected bool roundsReached;

        public RoundIterator(Match match)
        {
            MonitoringMatch = match;
            TargetRounds = MonitoringMatch.Config.GetValue<int>("mbc_match_rounds");
            CurrentRoundIdx = 0;
            RoundList = new List<Round>();
            roundsReached = false;
        }

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

        [XmlIgnore]
        public Round CurrentRound
        {
            get;
            protected set;
        }

        [XmlIgnore]
        public int CurrentRoundIdx
        {
            get;
            private set;
        }

        public Mode PlayMode
        {
            get;
            private set;
        }

        public List<Round> RoundList
        {
            get;
            private set;
        }

        [XmlIgnore]
        public bool TargetReached
        {
            get
            {
                return roundsReached & CurrentRound.Ended;
            }
        }

        public int TargetRounds
        {
            get;
            internal set;
        }

        [XmlIgnore]
        protected Match MonitoringMatch
        {
            get;
            private set;
        }

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

        internal bool PrevRound()
        {
            if (CurrentRoundIdx > 0)
            {
                CurrentRound = RoundList[--CurrentRoundIdx];
                return false;
            }
            return true;
        }

        protected abstract bool IsRoundsReached();

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