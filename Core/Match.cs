using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core
{
    /// <summary>
    /// Provides information about a matchup of controllers. The Match class sets and uses the following
    /// Configuration keys:
    /// <list type="bullet">    
    ///     <item><b>mbc_match_rounds_mode</b> - "all", "play to", or "best of".
    ///     <list type="bullet">
    ///         <item><b>all</b> - Runs N number of Rounds.</item>
    ///         <item><b>play to</b> - Runs rounds until a controller has reached N number of wins.</item>
    ///         <item><b>best of</b> - Tournament style; runs rounds until a controller has reached a number of wins that is then
    ///         impossible for the other controller to reach. Eg, 3 out of 5, 6 out of 13, 101 out of 200...
    ///         </item>
    ///     </list>
    ///     </item>
    ///     <item><b>mbc_match_rounds</b> - The number of rounds.</item>
    /// </list>
    /// Match objects internally orders each controller in the same order they are received in the constructors.
    /// Keep this in mind when accessing lists that are associated with the controllers in a Match or Round.
    /// </summary>
    public class Match
    {
        /// <summary>Sets default configuration values for keys that relate to this class.
        /// Should be called before using the global Configuration.Default object.</summary>
        /// <seealso cref="Configuration"/>
        public static void SetConfigDefaults()
        {
            Configuration.Default.SetValue<string>("mbc_match_rounds_mode", "all");
            Configuration.Default.SetValue<int>("mbc_match_rounds", 100);
        }

        private MatchInfo info;
        private PlayMode roundPlay;

        private List<Controller> controllers;
        private List<int> scores;

        private int totalRounds;
        private int currentRound;

        public Match(params Controller.ClassInfo[] controllers)
        {
            Init(Configuration.Global, controllers);
        }

        public Match(Configuration conf, params Controller.ClassInfo[] controllers)
        {
            Init(conf, controllers);
        }

        private void Init(Configuration conf, params Controller.ClassInfo[] controllers)
        {
            //Get the game info from the Configuration.
            //Expecting an exception to be thrown here if a controller isn't compatible with the configured game mode.
            info = new CMatchInfo(conf);

            //Populate the Match object's list of Controller objects present.
            this.controllers = new List<Controller>();
            foreach (var controller in controllers)
            {
                this.controllers.Add(new Controller(controller, info));
            }

            this.scores = new List<int>();

            //Configuration setting for the number of rounds this Match will perform.
            totalRounds = conf.GetValue<int>("mbc_match_rounds");
            currentRound = 0;

            //Configuration setting for round playing behaviour, given a number of rounds.
            switch (conf.GetValue<string>("mbc_match_rounds_mode"))
            {
                case "all":
                    roundPlay = PlayMode.AllRounds;
                    break;
                case "first to":
                    roundPlay = PlayMode.FirstTo;
                    break;
                case "best of":
                    roundPlay = PlayMode.BestOf;
                    break;
                default:
                    conf.SetValue<string>("mbc_match_rounds_mode", "all");
                    break;
            }
        }

        /// <summary>
        /// Gets the score of the controllers in this Match. The order of the scores is identical to the order
        /// of the controllers given when constructing this Match.
        /// </summary>
        public List<int> Scoreboard
        {
            get
            {
                return new List<int>(scores);
            }
        }

        /// <summary>
        /// Progresses the Match. Sets up a new Round when there is no Round in progress. Progresses the
        /// current Round in progress. Continues this until the criteria set by the PlayMode has been met.
        /// </summary>
        /// <returns>true if the Match is still in progress, false when the PlayMode criteria has been met.</returns>
        public bool Progress()
        {
            return true;
        }

        /// <summary>
        /// Gets the behaviour of rounds being played in this Match.
        /// </summary>
        public PlayMode RoundPlayMode
        {
            get
            {
                return roundPlay;
            }
        }

        /// <summary>
        /// Gets the MatchInfo object associated with this Match that describes the behaviour of the games
        /// by this Match.
        /// </summary>
        public MatchInfo Info
        {
            get
            {
                return info;
            }
        }

        /// <summary>
        /// Gets the number of rounds in this match.
        /// </summary>
        public int TotalRounds
        {
            get
            {
                return totalRounds;
            }
        }

        /// <summary>
        /// Gets round this Match is currently processing.
        /// </summary>
        public int CurrentRound
        {
            get
            {
                return currentRound;
            }
        }

        /// <summary>
        /// This enumerator identifies the behaviour of rounds being played out, given a number.
        /// </summary>
        public enum PlayMode
        {
            /// <summary>
            /// Runs N rounds.
            /// </summary>
            AllRounds,

            /// <summary>
            /// Runs rounds until a controller has reached N number of wins.
            /// </summary>
            FirstTo,

            /// <summary>
            /// Tournament style; runs rounds until a controller has reached a number of wins that is then
            /// impossible for the other controller to reach. Eg, 3 out of 5, 6 out of 13, 101 out of 200...
            /// </summary>
            BestOf
        }
    }
}
