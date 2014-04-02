using MBC.App.Terminal.Layouts;
using MBC.App.Terminal.Modules;
using MBC.Core;
using MBC.Core.Game;
using MBC.Core.Threading;
using MBC.Shared;
using MBC.Shared.Attributes;
using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace MBC.App.Terminal.Modules
{
    public class CompetitionRun : TerminalModule
    {
        private MatchCore competition;
        private string currentEventString;
        private StreamWriter fileWriter;
        private int millisDelay;
        private int roundsRun;
        private int turns;

        public CompetitionRun(MatchCore comp, int delay, bool eventsToFile)
        {
            turns = 0;
            roundsRun = 0;
            competition = comp;
            currentEventString = "";
            millisDelay = delay;
            comp.OnEvent += RoundEnd;
            comp.OnEvent += MatchEnd;
            comp.OnEvent += PlayerShot;
            comp.OnEvent += RoundTurn;
            comp.OnEvent += ShipDestroyed;
            /*
            if (eventsToFile)
            {
                fileWriter = new StreamWriter(Environment.CurrentDirectory + "\\..\\match_" + competition.ID + ".txt");
                fileWriter.WriteLine("START OF MATCH " + competition.ID);
            }*/
        }

        public void Begin()
        {
            new Thread(competition.Play).Start();
        }

        protected override void Display()
        {
            WriteHeader();
            WriteScoreRounds();
            WriteTurns();
            WriteCurrentEvent();
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void FileWriteEvent()
        {
            fileWriter.WriteLine(currentEventString);
        }

        [EventFilter(typeof(MatchEndEvent))]
        private void MatchEnd(Event ev)
        {
            BattleshipConsole.RemoveModule(this);
            BattleshipConsole.AddModule(new ResultDisplay(competition));
            BattleshipConsole.UpdateDisplay();
            if (fileWriter != null)
            {
                fileWriter.Close();
            }
        }

        [EventFilter(typeof(PlayerShotEvent))]
        private void PlayerShot(Event ev)
        {
            PlayerShotEvent evCasted = (PlayerShotEvent)ev;
            Shot shot = evCasted.Shot;
            Player plr = evCasted.Player;
            Ship shipHit = ShipList.GetShipAt(shot);
            if (shipHit != null)
            {
                currentEventString = string.Format("{0} hit {1}'s ship ({2}) at {3}",
                    plr,
                    shot.Receiver,
                    shipHit,
                    shot.Coordinates);
            }
            else
            {
                currentEventString = string.Format("{0} made a shot at {1} at coords {2}",
                    plr,
                    shot.Receiver,
                    shot.Coordinates);
            }
            WriteCurrentEvent();
        }

        [EventFilter(typeof(RoundEndEvent))]
        private void RoundEnd(Event ev)
        {
            turns = 0;
            roundsRun++;
            WriteScoreRounds();
            currentEventString = "!!!New round!!!";
        }

        private void RoundTurn(Event ev)
        {
            turns++;
            WriteTurns();
        }

        [EventFilter(typeof(ShipDestroyedEvent))]
        private void ShipDestroyed(Event ev)
        {
            ShipDestroyedEvent evCasted = (ShipDestroyedEvent)ev;
            currentEventString = string.Format("{0}'s ship ({1}) is destroyed",
                evCasted.Ship.Owner,
                evCasted.Ship);
            WriteCurrentEvent();
        }

        private void WriteCurrentEvent()
        {
            if (millisDelay > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                for (int i = 0; i < 4; i++)
                {
                    AlignToLine(8 + i);
                    WriteCharRepeat(' ', currentEventString.Length);
                }
                AlignToLine(8);
                WriteCenteredText(currentEventString);
                Thread.Sleep(millisDelay);
            }
            if (fileWriter != null)
            {
                FileWriteEvent();
            }
        }

        private void WriteHeader()
        {
            AlignToLine(0);
            Console.ForegroundColor = ConsoleColor.White;
            WriteCenteredText("=====COMPETITION IN PROGRESS=====");
            AlignToLine(2);
            var players = competition.Players.ToArray();
            WriteCenteredText(players[0] + " vs. " + players[1]);
        }

        private void WriteScoreRounds()
        {
            Console.ForegroundColor = ConsoleColor.White;
            AlignToLine(3);
            var players = competition.Players.ToArray();
            WriteCenteredText("[" + players[0].Score + "]     score     [" + players[0].Score + "]");
            NewLine();
            WriteCenteredText(roundsRun + " rounds out of " + competition.NumberOfRounds);
        }

        private void WriteTurns()
        {
            AlignToLine(6);
            Console.ForegroundColor = ConsoleColor.Red;
            WriteCenteredText(turns + " turns made this round.");
        }
    }
}