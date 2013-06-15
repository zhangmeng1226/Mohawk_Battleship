using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBC.App.BattleshipConsole
{
    public static class Show
    {

        public static void Config()
        {
            Console.WriteLine("[Key] = [Value]");
            foreach (var pair in Input.Configuration.GetPairs())
            {
                Console.Write(pair.Key);
                Console.Write(" = ");
                Console.WriteLine(pair.Value);
            }
        }

        public static void Controllers()
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

        public static void Parse(string[] commandParams, ref int idx)
        {
            switch (commandParams[idx++].ToLower())
            {
                case "config":
                    Config();
                    break;
                case "bots":
                case "controllers":
                    Controllers();
                    break;
            }
        }
    }
}
