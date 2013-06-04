using MBC.App.Terminal.Layouts;
using MBC.App.Terminal.Modules;
using MBC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Modules
{
    public class CompetitionRun : TerminalModule
    {
        Competition competition;
        private int roundsRun;
        private int totalRounds;
        private int turns;

        public CompetitionRun(Competition comp, int rounds)
        {
            roundsRun = 0;
            totalRounds = rounds;
            turns = 0;
            competition = comp;
            competition.RoundEndEvent += CompRoundEnd;
            competition.RoundTurnEndEvent += CompRoundTurn;
        }

        private void CompRoundEnd()
        {
            lock (this)
            {
                turns = 0;
                roundsRun++;
                BattleshipConsole.UpdateDisplay();
            }
        }

        private void CompRoundTurn()
        {
            lock (this)
            {
                Utility.StoreConsoleColors();
                turns++;
                Utility.SetConsoleForegroundColor("Red");
                WriteCenteredText(turns + " turns made this round.");
                Utility.RestoreConsoleColors();
            }
        }

        public void Begin()
        {
            competition.RunCompetitionThread();
        }

        protected override void Display()
        {
            lock (this)
            {
                if (roundsRun == totalRounds)
                {
                    BattleshipConsole.RemoveModule(this);
                    BattleshipConsole.AddModule(new ResultDisplay(competition));
                    BattleshipConsole.UpdateDisplay();
                    return;
                }
                Utility.SetConsoleForegroundColor("White");
                WriteCenteredText("=====COMPETITION IN PROGRESS=====");
                NewLine(2);

                Field.ControllerInfo redInfo = competition.GetBattlefield()[Controller.Red];
                Field.ControllerInfo blueInfo = competition.GetBattlefield()[Controller.Blue];

                WriteCenteredText(redInfo.name + " " + redInfo.version + " vs. " + blueInfo.name + " " + blueInfo.version);
                NewLine();
                WriteCenteredText("[" + redInfo.score + "]     score     [" + blueInfo.score + "]");
                NewLine();
                WriteCenteredText(roundsRun + " rounds out of " + totalRounds);
                NewLine(2);
                Utility.SetConsoleForegroundColor("Red");
                WriteCenteredText(turns + " turns made this round.");
            }
        }
    }
}
