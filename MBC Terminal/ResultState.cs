using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.Core;

namespace MBC.Terminal
{
    public class ResultState : ConsoleState
    {


        Dictionary<IBattleshipController, int> results;

        public ResultState(BattleshipConsole main, Dictionary<IBattleshipController, int> results)
            : base(main)
        {
            this.results = results;
            extraMenu = "[R]ematch, [S]election";
        }

        protected override void StateDisplay()
        {
            WriteCenteredText("Battleship competition results", headerEnds);
            foreach (var key in results.Keys.OrderByDescending(k => results[k]))
                Console.WriteLine(key.Name + " has scored " + results[key]);
        }

        protected override ConsoleState Response(string input)
        {
            switch (input)
            {
                case "R":
                    return new CompetitionState(main, results.Keys.ToArray());
                case "S":
                    return new SelectorState(main);
            }
            return this;
        }
    }
}
