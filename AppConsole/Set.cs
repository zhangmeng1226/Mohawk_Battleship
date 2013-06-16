using MBC.Core.Util;
using System;

namespace MBC.App.BattleshipConsole
{
    public static class Set
    {
        public static void Config(string[] commandParams, ref int idx)
        {
            var key = commandParams[idx++];
            var valueUnparsed = commandParams[idx++];

            if (!Configuration.Global.SetValue(key, valueUnparsed))
            {
                Console.WriteLine("The given value is invalid for the given key.");
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
    }
}