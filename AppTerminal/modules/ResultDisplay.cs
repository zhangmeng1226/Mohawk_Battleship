using MBC.App.Terminal.Controls;
using MBC.App.Terminal.Layouts;
using MBC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Modules
{
    public class ResultDisplay : ConsoleModule
    {
        /*
        Competition competition;
        FlowLayout buttonLayout;

        public ResultDisplay(Competition comp)
        {
            competition = comp;

            buttonLayout = new FlowLayout(FlowLayout.Alignment.Center);
            buttonLayout.Add(new ButtonControl("Back to Main Menu", ButtonClick));

            AddControlLayout(buttonLayout);
        }

        private bool ButtonClick(string txt)
        {
            BattleshipConsole.AddModule(new MainMenu());
            BattleshipConsole.RemoveModule(this);
            BattleshipConsole.UpdateDisplay();
            return false;
        }

        protected override void Display()
        {
            WriteCenteredText("=====COMPETITION RESULTS=====");
            NewLine(2);
            WriteCenteredText("The result...");
            NewLine();

            Field.ControllerInfo redInfo = competition.GetBattlefield()[Controller.Red];
            Field.ControllerInfo blueInfo = competition.GetBattlefield()[Controller.Blue];
            if (redInfo.score > blueInfo.score)
            {
                WriteCenteredText(redInfo.name + " won!");
            }
            else if (blueInfo.score > redInfo.score)
            {
                WriteCenteredText(blueInfo.name + " won!");
            }
            else
            {
                WriteCenteredText("DRAW");
            }
            NewLine(2);
            buttonLayout.Display();
        }*/
    }
}
