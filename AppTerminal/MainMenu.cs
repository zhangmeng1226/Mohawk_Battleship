using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.Core;
using MBC.App.Terminal.Layouts;
using MBC.App.Terminal.Controls;

namespace MBC.App.Terminal
{

    /**
     * <summary>This MainMenu class is the first object to be displayed on the console when the application
     * is started.
     * 
     * 
     * </summary>
     */
    public class MainMenu : TerminalModule
    {
        VerticalLayout menuLayout;
        public MainMenu()
        {
            menuLayout = new VerticalLayout(VerticalLayout.VerticalAlign.Center);
            menuLayout.Add(new ButtonControl("Start a competition", MenuSelectEvent));
            menuLayout.Add(new ButtonControl("Configuration Manager", MenuSelectEvent));
            menuLayout.Add(new ButtonControl("Exit", MenuSelectEvent));
            AddControlLayout(menuLayout);
        }

        void MenuSelectEvent(string s)
        {
            switch (s)
            {
                case "Start a competition":
                    BattleshipConsole.RemoveModule(this);
                    BattleshipConsole.AddModule(new BotSelector());
                    BattleshipConsole.UpdateDisplay();
                    break;
                case "Configuration Manager":
                    break;
                case "Exit":
                    BattleshipConsole.Running = false;
                    break;
            }
        }

        protected override void Display()
        {
            WriteCenteredText("=====MAIN MENU=====");
            NewLine(2);
            WriteCenteredText("This is the MBC Main Menu.");
            NewLine();
            WriteCenteredText("Use the arrow keys to navigate menus.");
            NewLine(2);
            menuLayout.Display();
        }
    }
}
