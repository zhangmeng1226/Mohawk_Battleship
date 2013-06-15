using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBC.App.BattleshipConsole
{
    public static class Help
    {

        public static void Display()
        {
            Console.WriteLine("The available commands in this console application are the following:");
            Console.WriteLine("show [controllers|config] - Shows loaded controllers or configuration.");
            Console.WriteLine("set [config] - Sets the configuration");
            Console.WriteLine("set config [key] [value] - Sets a key in the configuration to the value");
            Console.WriteLine("match [controllers...] - Create a new match with the current configuration and given controllers.");
            Console.WriteLine("step - Steps through the match by one iteration.");
            Console.WriteLine("start - Runs through the match until its over.");
            Console.WriteLine("exit - Exit the application / return to the console.");
            Console.WriteLine();
            Console.WriteLine("A running match can be stopped with the CTRL - C combination without exiting the application.");
            Console.WriteLine("The application can be run with chained parameters. Eg, mbc_console.exe match 1 2 3 start match 4 2 start");
        }
    }
}
