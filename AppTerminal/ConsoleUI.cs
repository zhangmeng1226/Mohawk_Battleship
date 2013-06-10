using MBC.App.Terminal.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal
{
    public class ConsoleUI
    {
        private RootComponent rootComponent;
        private static bool isRunning = true; //If the application is running.

        private static string inputLine = ""; //A string of input given by the user.
        private static long lastTickBeep = 0; //Timer used to delay subsequent Console.Beep() invokes.

        /// <summary>Gets or sets the running state of this application. Setting this false will cause
        /// the application to quit.</summary>
        public static bool Running
        {
            get { return isRunning; }
            set { isRunning = value; }
        }

    }
}
