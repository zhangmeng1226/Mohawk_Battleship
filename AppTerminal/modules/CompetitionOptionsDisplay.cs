using MBC.App.Terminal.Controls;
using MBC.App.Terminal.Layouts;
using MBC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Modules
{
    public class CompetitionOptionsDisplay : ConsoleModule
    {
        IBattleshipController[] controllers;
        FlowLayout layout;
        NumericControl roundsNumberControl;

        public CompetitionOptionsDisplay(IBattleshipController red, IBattleshipController blue)
        {
            Init(new IBattleshipController[] {red, blue});
        }

        public CompetitionOptionsDisplay(IBattleshipController[] ctrls)
        {
            Init(ctrls);
        }

        private void Init(IBattleshipController[] ctrls)
        {
            controllers = ctrls;
            layout = new FlowLayout(FlowLayout.Alignment.Center);
            roundsNumberControl = new NumericControl(new NumericControl.NumericControlParameters("# of rounds", false, 1, 100000, 1, 50));
            layout.Add(roundsNumberControl);
            layout.Add(new CheckboxControl("Play out rounds"));
            layout.Add(new ButtonControl("Confirm", ButtonConfirmEvent));
            AddControlLayout(layout);
        }

        private bool ButtonConfirmEvent(string btn)
        {
            Configuration.Global.SetValue<int>("mbc_rounds", roundsNumberControl.Value);
            Competition comp = new Competition(controllers, Configuration.Global);
            CompetitionRun runner = new CompetitionRun(comp, roundsNumberControl.Value);
            BattleshipConsole.AddModule(runner);
            BattleshipConsole.RemoveModule(this);
            BattleshipConsole.UpdateDisplay();
            runner.Begin();
            return true;
        }

        protected override void Display()
        {
            Utility.SetConsoleForegroundColor("White");
            WriteCenteredText("=====COMPETITION SETTINGS=====");
            NewLine();

            Utility.SetConsoleForegroundColor("Red");
            WriteText(controllers[Controller.Red].Name);

            Utility.SetConsoleForegroundColor("Green");
            WriteCenteredText("VS.");

            Utility.SetConsoleForegroundColor("Blue");
            AlignToCoord(Width - controllers[Controller.Blue].Name.Length - 1, CurrentY);
            WriteText(controllers[Controller.Blue].Name);
            NewLine();

            Utility.SetConsoleForegroundColor("White");
            WriteCenteredText("Review the available settings for this matchup.");
            NewLine(2);
            layout.Display();
        }
    }
}
