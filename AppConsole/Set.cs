using MBC.Core.Util;
using System;
using System.Collections.Generic;

namespace MBC.App.BattleshipConsole
{
    /// <summary>
    /// Contains functions for modifying variables within the application.
    /// </summary>
    public static class Set
    {
        /// <summary>
        /// A <see cref="MBCShellCommandHandler"/> that is used to set the value in
        /// a given key to the one in the input. Outputs an indication of success
        /// to the console afterwards.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
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
            catch
            {
                Console.WriteLine(key+" was not set to "+valueUnparsed+".");
            }
        }
    }
}