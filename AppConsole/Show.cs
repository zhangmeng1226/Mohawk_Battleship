using MBC.Core.Util;
using System;

namespace MBC.App.BattleshipConsole
{
    /// <summary>
    /// Contains functions for showing various information loaded in the application.
    /// </summary>
    public static class Show
    {
        /// <summary>
        /// Shows the <see cref="Configuration"/> settings that are available and
        /// their current values to the console.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Config(int idx, params string[] param)
        {
            Console.WriteLine("[Key] = [Value]");
            foreach (var key in Configuration.GetAllKnownKeys())
            {
                Console.Write(key);
                Console.Write(" = ");
                Console.WriteLine(Configuration.Global.GetValueString(key));
            }
        }

        /// <summary>
        /// Shows the <see cref="Core.ControllerInformation"/> that has been loaded into <see cref="Input"/>
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Controllers(int idx, params string[] param)
        {
            if (Input.Controllers.Count == 0)
            {
                Console.WriteLine("No controllers available.");
                return;
            }
            Console.WriteLine("Available controllers:");
            for (var i = 0; i < Input.Controllers.Count; i++)
            {
                Console.Write('[');
                Console.Write(i);
                Console.Write("]: ");
                Console.WriteLine(Input.Controllers[i]);
            }
        }

        /// <summary>
        /// Writes a string of text in the console indented at <paramref name="cursorLeft"/>
        /// with no breaks in each word that is in the <paramref name="txt"/>.
        /// </summary>
        /// <param name="cursorLeft">The column on the console to write to.</param>
        /// <param name="txt">A string of text that is to be written.</param>
        public static void ProperLine(int cursorLeft, string txt)
        {
            var txtSplit = txt.Split(' ');
            var targetWidth = Console.WindowWidth - cursorLeft;
            var currentWidth = 0;
            for (var txtIdx = 0; txtIdx < txtSplit.Length; txtIdx++)
            {
                currentWidth += txtSplit[txtIdx].Length + 1;
                if (currentWidth > targetWidth)
                {
                    Console.WriteLine();
                    Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                    currentWidth = txtSplit[txtIdx].Length + 1;
                }
            }
        }
    }
}