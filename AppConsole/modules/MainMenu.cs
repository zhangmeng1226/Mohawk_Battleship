using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.App.Terminal.Controls;
using MBC.App.Terminal.Layouts;
using MBC.Core;

namespace MBC.App.Terminal.Modules
{
    public class MainMenu : TerminalModule
    {
        private VerticalLayout menuLayout;

        public MainMenu()
        {
            menuLayout = new VerticalLayout(VerticalLayout.VerticalAlign.Center);
            menuLayout.Add(new ButtonControl("Start a competition", MenuSelectEvent));
            //menuLayout.Add(new ButtonControl("Configuration Manager", MenuSelectEvent));
            menuLayout.Add(new ButtonControl("Exit", MenuSelectEvent));
            AddControlLayout(menuLayout);
        }

        protected override void Display()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteCenteredText("Version: 0.5.1  Date: Jan 16, 2014");
            NewLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            WriteCenteredText("=====MAIN MENU=====");
            NewLine(2);
            WriteCenteredText("This is the MBC Main Menu.");
            NewLine();
            WriteCenteredText("Use the arrow keys to navigate menus.");
            NewLine(2);
            menuLayout.Display();
        }

        private bool MenuSelectEvent(string s)
        {
            switch (s)
            {
                case "Start a competition":
                    BattleshipConsole.RemoveModule(this);
                    BattleshipConsole.AddModule(new BotSelector());
                    BattleshipConsole.UpdateDisplay();
                    return true;

                case "Configuration Manager":
                    BattleshipConsole.RemoveModule(this);
                    BattleshipConsole.AddModule(new ConfigurationManager());
                    BattleshipConsole.UpdateDisplay();
                    return true;

                case "Exit":
                    BattleshipConsole.Running = false;
                    return true;
            }
            return false;
        }
    }
}