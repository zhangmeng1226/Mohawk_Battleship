using MBC.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBC.App.BattleshipConsole
{
    public static class Show
    {

        public static void Config(int idx, params string[] param)
        {
            Console.WriteLine("[Key] = [Value]");
            foreach (var pair in Configuration.Global.GetPairs())
            {
                Console.Write(pair.Key);
                Console.Write(" = ");
                Console.WriteLine(pair.Value);
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
    }
}
