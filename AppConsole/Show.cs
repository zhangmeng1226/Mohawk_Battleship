using MBC.Core.Util;
using System;

namespace MBC.App.BattleshipConsole
{
    public static class Show
    {
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