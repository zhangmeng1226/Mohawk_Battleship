using MBC.App.Terminal.Controls;
using MBC.App.Terminal.Layouts;
using MBC.Core;
using MBC.Core.Game;
using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Modules
{
    public class ResultDisplay : TerminalModule
    {
        private VerticalLayout buttonLayout;
        private MatchCore competition;

        public ResultDisplay(MatchCore comp)
        {
            competition = comp;

            buttonLayout = new VerticalLayout(VerticalLayout.VerticalAlign.Center);
            buttonLayout.Add(new ButtonControl("Back to Main Menu", ButtonClick));

            AddControlLayout(buttonLayout);
        }

        protected override void Display()
        {
            Console.ForegroundColor = ConsoleColor.White;
            WriteCenteredText("=====COMPETITION RESULTS=====");
            NewLine(2);

            WriteCenteredText(string.Format("{0}                 vs.               {1}", competition.Players.First(), competition.Players.Last()));
            NewLine();
            WriteCenteredText(string.Format("{0} wins                                  {1} wins", competition.Players.First(), competition.Players.Last()));

            NewLine();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.BackgroundColor = ConsoleColor.White;
            WriteCenteredText("POST SUMMARY");
            NewLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            WriteCenteredText(string.Format("{0} total games played", competition.NumberOfRounds));
            NewLine();

            var shotBots = new List<int>();
            var hitBots = new List<int>();
            shotBots.AddRange(new int[] { 0, 0 });
            hitBots.AddRange(new int[] { 0, 0 });
            Event lastEvent = null;
            foreach (var ev in competition.Events)
            {
                var shotEvent = ev as PlayerShotEvent;
                var hitEvent = ev as ShipHitEvent;
                if (shotEvent != null)
                {
                    shotBots[shotEvent.Player.ID]++;
                }
                else if (hitEvent != null)
                {
                    hitBots[hitEvent.Ship.Owner.ID]++;
                }
                lastEvent = ev;
            }
            for (int i = 0; i < 2; i++)
            {
                shotBots[i] /= competition.NumberOfRounds;
                hitBots[i] /= competition.NumberOfRounds;
            }
            WriteCenteredText(string.Format("{0}     shots on average     {1}", shotBots[0], shotBots[1]));
            NewLine();
            WriteCenteredText(string.Format("{0}     hits on average      {1}", hitBots[0], hitBots[1]));
            NewLine();
            WriteCenteredText(string.Format("{0}     misses on average    {1}", shotBots[0] - hitBots[0], shotBots[1] - hitBots[1]));
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