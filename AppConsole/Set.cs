using MBC.Core.Util;
using System;
using System.Collections.Generic;

namespace MBC.App.BattleshipConsole
{
    public static class Set
    {
        public static void Config(int idx, params string[] param)
        {
            var key = param[idx++];
            var valueUnparsed = param[idx++];
            try
            {
                Configuration.Global.SetValue(key, valueUnparsed);
                Console.Write("Configuration key \"");
                Console.Write(key);
                Console.Write("\" set to \"");
                Console.Write(valueUnparsed);
                Console.WriteLine("\"");
            }
            catch (Exception ex)
            {
                Console.WriteLine(key+" was not set to "+valueUnparsed+".");
            }
        }
    }
}