using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.App.Terminal.Controls;
using MBC.App.Terminal.Layouts;
using MBC.Core;
using MBC.Core.Matches;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;

namespace MBC.App.Terminal.Modules
{
    public class CompetitionOptionsDisplay : TerminalModule
    {
        private IController blue;
        private CheckboxControl dumpEventsCheck;
        private VerticalLayout layout;
        private NumericControl millisecondControl;
        private IController red;
        private NumericControl roundsNumberControl;

        public CompetitionOptionsDisplay(IController red, IController blue)
        {
            this.red = red;
            this.blue = blue;
            layout = new VerticalLayout(VerticalLayout.VerticalAlign.Center);
            roundsNumberControl = new NumericControl(new NumericControl.NumericControlParameters("# of rounds", false, 1, 100000, 1, 1000));
            millisecondControl = new NumericControl(new NumericControl.NumericControlParameters("Millisecond delay", false, 0, 10000, 10, 0));
            dumpEventsCheck = new CheckboxControl("Dump shots to file");
            layout.Add(roundsNumberControl);
            //layout.Add(new CheckboxControl("Play out rounds"));
            layout.Add(millisecondControl);
            layout.Add(dumpEventsCheck);
            layout.Add(new ButtonControl("Confirm", ButtonConfirmEvent));
            AddControlLayout(layout);
        }

        protected override void Display()
        {
            var redName = red.GetAttribute<NameAttribute>().Name;
            var blueName = blue.GetAttribute<NameAttribute>().Name;
            Console.ForegroundColor = ConsoleColor.White;
            WriteCenteredText("=====COMPETITION SETTINGS=====");
            NewLine();

            Console.ForegroundColor = ConsoleColor.Red;
            WriteText(redName);

            Console.ForegroundColor = ConsoleColor.Green;
            WriteCenteredText("VS.");

            Console.ForegroundColor = ConsoleColor.Blue;
            AlignToCoord(Width - blueName.Length - 1, CurrentY);
            WriteText(blueName);
            NewLine();

            Console.ForegroundColor = ConsoleColor.White;
            WriteCenteredText("Review the available settings for this matchup.");
            NewLine(2);
            layout.Display();
        }

        private bool ButtonConfirmEvent(string btn)
        {
            Configuration.Global.SetValue("mbc_match_rounds", roundsNumberControl.Value.ToString());
            var match = new AllRoundsMatch();
            match.AddController(red);
            match.AddController(blue);
            CompetitionRun runner = new CompetitionRun(match, millisecondControl.Value, dumpEventsCheck.IsChecked);
            BattleshipConsole.AddModule(runner);
            BattleshipConsole.RemoveModule(this);
            BattleshipConsole.UpdateDisplay();
            runner.Begin();
            return true;
        }
    }
}