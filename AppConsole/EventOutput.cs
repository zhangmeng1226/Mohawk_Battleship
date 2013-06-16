using MBC.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBC.App.BattleshipConsole
{
    public static class EventOutput
    {

        public static void Enable(int idx, params string[] param)
        {
            if (MatchRun.CurrentMatch == null)
            {
                Console.WriteLine("No match created. Cannot enable events.");
                return;
            }

            switch (param[idx])
            {
                case "match":
                    MatchRun.CurrentMatch.MatchEvent += MatchEventOutput;
                    break;
                case "controller":
                    MatchRun.CurrentMatch.ControllerEvent += ControllerEventOutput;
                    break;
                case "round":
                    MatchRun.CurrentMatch.RoundEvent += RoundEventOutput;
                    break;
                default:
                    Console.WriteLine("Invalid event specified.");
                    break;
            }
        }

        public static void Disable(int idx, params string[] param)
        {
            if (MatchRun.CurrentMatch == null)
            {
                Console.WriteLine("No match created. Cannot disable events.");
                return;
            }

            switch (param[idx])
            {
                case "match":
                    MatchRun.CurrentMatch.MatchEvent -= MatchEventOutput;
                    break;
                case "controller":
                    MatchRun.CurrentMatch.ControllerEvent -= ControllerEventOutput;
                    break;
                case "round":
                    MatchRun.CurrentMatch.RoundEvent -= RoundEventOutput;
                    break;
                default:
                    Console.WriteLine("Invalid event specified.");
                    break;
            }
        }

        private static void MatchEventOutput(MatchEvent ev)
        {
            Console.WriteLine(ev);
        }

        private static void RoundEventOutput(RoundEvent ev)
        {
            Console.WriteLine(ev);
        }

        private static void ControllerEventOutput(ControllerEvent ev)
        {
            Console.WriteLine(ev);
        }
    }
}
