using System;
using System.Collections.Generic;
using MBC.Core.Events;

namespace MBC.App.BattleshipConsole
{
    /// <summary>
    /// Provides functions that relate to outputting events in the console.
    /// </summary>
    public static class EventOutput
    {
        private static HashSet<string> eventOutputClasses = new HashSet<string>();

        /// <summary>
        /// Enables the output of a specific event type that is generated in a <see cref="Core.Match"/>.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Disable(int idx, params string[] param)
        {
            if (MatchRun.CurrentMatch == null)
            {
                Console.WriteLine("No match created. Cannot disable events.");
                return;
            }

            eventOutputClasses.Remove(param[idx]);
            Console.WriteLine("Events with name " + param[idx] + " will no longer output.");
        }

        /// <summary>
        /// Enables the output of a specific event type that is generated in a <see cref="Core.Match"/>.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Enable(int idx, params string[] param)
        {
            if (MatchRun.CurrentMatch == null)
            {
                Console.WriteLine("No match created. Cannot enable events.");
                return;
            }
            eventOutputClasses.Add(param[idx]);
            Console.WriteLine("Events with name " + param[idx] + " will output.");
        }

        public static void EventGenerated(Event ev)
        {
            Console.WriteLine(ev.GetType().Name);
        }
    }
}