using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.Core;

namespace MBC.App.Terminal
{

    /**
     * <summary>This MainMenu class is the first object to be displayed on the console when the application
     * is started.</summary>
     */
    public class MainMenu : TerminalModule
    {
        Menu menu;
        public MainMenu()
        {
            menu = new Menu(this, 6, true, "Start a competition", "Configuration Manager", "Exit");
            menu.MenuSelectEvent += new Menu.MenuSelect(menu_MenuSelectEvent);
        }

        void menu_MenuSelectEvent(string s)
        {
            switch (s)
            {
                case "Start a competition":
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
            NewLine();
            menu.Display();
        }
    }
}
