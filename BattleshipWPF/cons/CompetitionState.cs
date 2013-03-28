using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship
{
    public class CompetitionState : ConsoleState
    {
        private IBattleshipOpponent[] bots;
        private Dictionary<IBattleshipOpponent, int> scores;

        public CompetitionState(BattleshipConsole main, IBattleshipOpponent[] opp)
            : base(main)
        {
            extraMenu = "[S]election";
            bots = opp;
        }

        protected override void StateDisplay()
        {
            WriteCenteredText("Bot competition mode", headerEnds);
            Console.WriteLine("Running the competition...\n\n");
            BattleshipCompetition bc = new BattleshipCompetition(bots, main.Config);
            scores = bc.RunCompetition();
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
