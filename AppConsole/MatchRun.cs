using MBC.Core;
using MBC.Core.Matches;
using MBC.Core.Util;
using System;
using MBC.Core.Rounds;

namespace MBC.App.BattleshipConsole
{
    /// <summary>
    /// Provides functions and variables for running a <see cref="Match"/>.
    /// </summary>
    [Configuration("mbc_console_match_show_events", true)]
    public static class MatchRun
    {
        /// <summary>
        /// Gets or sets <see cref="Match"/> that is currently being used in the application.
        /// </summary>
        public static Match CurrentMatch { get; set; }

        /// <summary>
        /// Gets or sets the continuous running state of a <see cref="Match"/>.
        /// </summary>
        public static bool Running { get; set; }

        /// <summary>
        /// Progresses the <see cref="MatchRun.CurrentMatch"/> by a single step.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Step(int idx, params string[] param)
        {
            if (CurrentMatch == null)
            {
                Console.WriteLine("No match has been created.");
                return;
            }

            if (CurrentMatch.StepForward())
            {
                Console.WriteLine("The match is over.");
            }
        }

        /// <summary>
        /// Begins running the <see cref="MatchRun.CurrentMatch"/> continuously until it ends, or
        /// <see cref="MatchRun.Running"/> is made false.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Start(int idx, params string[] param)
        {
            int bot1 = 0;
            int bot2 = 0;

            if (CurrentMatch == null)
            {
                Console.WriteLine("No match has been created.");
                return;
            }
            Running = true;
            while (Running && !CurrentMatch.StepForward());
            Console.WriteLine("The match has stopped.");


            foreach (Round currentRound in CurrentMatch.Rounds.RoundList)
            {
                var winner = currentRound.Remaining;

                if (winner[0] == 0)
                {
                    bot1++;
                }
                else
                {
                    bot2++;
                }
            }

            Console.WriteLine("0: has " + bot1 + " wins");
            Console.WriteLine("1: has " + bot2 + " wins");
        }

        /// <summary>
        /// Stops and ends the <see cref="MatchRun.CurrentMatch"/>.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Stop(int idx, params string[] param)
        {
            CurrentMatch.End();
            Console.WriteLine("The match has been stopped and ended.");                        
        }
    }
}