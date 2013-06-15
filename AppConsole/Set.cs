using MBC.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBC.App.BattleshipConsole
{
    public static class Set
    {

        public static void Config(string[] commandParams, ref int idx)
        {
            var key = commandParams[idx++];
            var valueUnparsed = commandParams[idx++];

            if (!Input.Configuration.SetValue(key, valueUnparsed))
            {
                Console.WriteLine("The given value type differs from the required type of "+Configuration.ParseString(valueUnparsed).GetType().ToString());
            }
            else
            {
                Console.Write("Configuration key \"");
                Console.Write(key);
                Console.Write("\" set to \"");
                Console.Write(valueUnparsed);
                Console.WriteLine("\"");
            }
        }

        public static void Parse(string[] commandParams, ref int idx)
        {
            switch (commandParams[idx++])
            {
                case "config":
                    Config(commandParams, ref idx);
                    break;
                default:
                    Console.WriteLine("Nothing set.");
                    break;
            }
        }
    }
}
