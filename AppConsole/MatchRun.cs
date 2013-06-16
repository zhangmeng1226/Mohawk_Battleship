using MBC.Core;
using MBC.Core.Events;
using MBC.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MBC.App.BattleshipConsole
{
    [Configuration("mbc_console_match_show_events", true)]
    public static class MatchRun
    {
        public static Match CurrentMatch { get; set; }
        public static bool Running { get; set; }

        public static void Step(int idx, params string[] param)
        {
            if (CurrentMatch == null)
            {
                Console.WriteLine("No match has been created.");
                return;
            }

            if (!CurrentMatch.Step())
            {
                Console.WriteLine("The match is over.");
            }
        }

        public static void Start(int idx, params string[] param)
        {
            if (CurrentMatch == null)
            {
                Console.WriteLine("No match has been created.");
                return;
            }
            Running = true;
            while (Running && CurrentMatch.Step()) ;
            Console.WriteLine("The match has stopped.");
        }

        public static void Stop(int idx, params string[] param)
        {
            CurrentMatch.End();
            Console.WriteLine("The match has been stopped and removed.");
        }
    }
}
