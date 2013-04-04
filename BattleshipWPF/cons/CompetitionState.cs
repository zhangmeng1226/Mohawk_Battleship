using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship
{
    public class CompetitionState : ConsoleState
    {
        private IBattleshipController[] bots;
        private Dictionary<IBattleshipController, int> scores;

        public CompetitionState(BattleshipConsole main, IBattleshipController[] ibc)
            : base(main)
        {
            extraMenu = "[S]election";
            bots = ibc;
        }

        protected override void StateDisplay()
        {
            WriteCenteredText("Bot competition mode", headerEnds);
            Console.WriteLine("Running the competition...\n\n");
            MBCCompetition bc = new MBCCompetition(bots, main.Config);
            bc.RunCompetition();
            scores = bc.GetScores();
            Console.WriteLine("Done! Press any key to view the final results...");
        }

        protected override ConsoleState Response(string input)
        {
            switch (input)
            {
                case "S":
                    return new SelectorState(main);
            }
            return new ResultState(main, scores);
        }
    }
}
