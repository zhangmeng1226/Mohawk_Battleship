using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.App.Terminal.Layouts;
using MBC.App.Terminal.Modules;
using MBC.Core;
using MBC.Core.Events;
using MBC.Core.Matches;
using MBC.Core.Threading;

namespace MBC.App.Terminal.Modules
{
    public class CompetitionRun : TerminalModule
    {
        private ActiveMatch competition;
        private int roundsRun;
        private FuncThreader threader;
        private int turns;

        public CompetitionRun(ActiveMatch comp)
        {
            turns = 0;
            roundsRun = 0;
            competition = comp;
            competition.AddEventAction(typeof(PlayerShotEvent), CompRoundTurn);
            competition.AddEventAction(typeof(RoundEndEvent), CompRoundEnd);
            threader = new FuncThreader();
        }

        public void Begin()
        {
            threader.RunMethod(new Action(competition.Play));
        }

        protected override void Display()
        {
            lock (this)
            {
                if (roundsRun == competition.CompiledConfig.NumberOfRounds)
                {
                    BattleshipConsole.RemoveModule(this);
                    BattleshipConsole.AddModule(new ResultDisplay(competition));
                    BattleshipConsole.UpdateDisplay();
                    return;
                }
                Console.ForegroundColor = ConsoleColor.White;
                WriteCenteredText("=====COMPETITION IN PROGRESS=====");
                NewLine(2);

                WriteCenteredText(competition.Registers[0] + " vs. " + competition.Registers[1]);
                NewLine();
                WriteCenteredText("[" + competition.Registers[0].Score + "]     score     [" + competition.Registers[1].Score + "]");
                NewLine();
                WriteCenteredText(roundsRun + " rounds out of " + competition.CompiledConfig.NumberOfRounds);
                NewLine(2);
                Console.ForegroundColor = ConsoleColor.Red;
                WriteCenteredText(turns + " turns made this round.");
            }
        }

        private void CompRoundEnd(Event ev)
        {
            lock (this)
            {
                turns = 0;
                roundsRun++;
                BattleshipConsole.UpdateDisplay();
            }
        }

        private void CompRoundTurn(Event ev)
        {
            lock (this)
            {
                var colorStore = ColorStore.StoreCurrentColors();
                turns++;
                Console.ForegroundColor = ConsoleColor.Red;
                WriteCenteredText(turns + " turns made this round.");
                colorStore.Restore();
            }
        }
    }
}