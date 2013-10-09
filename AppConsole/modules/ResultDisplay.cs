using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.App.Terminal.Controls;
using MBC.App.Terminal.Layouts;
using MBC.Core;
using MBC.Core.Matches;

namespace MBC.App.Terminal.Modules
{
    public class ResultDisplay : TerminalModule
    {
        private VerticalLayout buttonLayout;
        private Match competition;

        public ResultDisplay(Match comp)
        {
            competition = comp;

            buttonLayout = new VerticalLayout(VerticalLayout.VerticalAlign.Center);
            buttonLayout.Add(new ButtonControl("Back to Main Menu", ButtonClick));

            AddControlLayout(buttonLayout);
        }

        protected override void Display()
        {
            WriteCenteredText("=====COMPETITION RESULTS=====");
            NewLine(2);
            WriteCenteredText("The result...");
            NewLine();

            if (competition.Registers[0].Score > competition.Registers[1].Score)
            {
                WriteCenteredText(competition.Registers[0] + " won!");
            }
            else if (competition.Registers[1].Score > competition.Registers[0].Score)
            {
                WriteCenteredText(competition.Registers[1] + " won!");
            }
            else
            {
                WriteCenteredText("DRAW");
            }
            NewLine(2);
            buttonLayout.Display();
        }

        private bool ButtonClick(string txt)
        {
            BattleshipConsole.AddModule(new MainMenu());
            BattleshipConsole.RemoveModule(this);
            BattleshipConsole.UpdateDisplay();
            return false;
        }
    }
}