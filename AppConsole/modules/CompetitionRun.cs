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
        private int boardsBaseY;
        private MatchCore competition;
        private string currentEventString;
        private StreamWriter fileWriter;
        private bool isWritten;
        private int lastMillis;
        private int millisDelay;
        private int roundsRun;
        private int turns;

        public CompetitionRun(MatchCore comp, int delay, bool eventsToFile)
        {
            isWritten = false;
            boardsBaseY = -1;
            turns = 0;
            roundsRun = 0;
            competition = comp;
            currentEventString = "";
            millisDelay = delay;

            comp.OnEvent += RoundEnd;
            comp.OnEvent += MatchEnd;
            comp.OnEvent += LastEvent;
            comp.OnEvent += RoundTurn;

            if (millisDelay > 0)
            {
                comp.OnEvent += ASCIIUpdateShot;
                comp.OnEvent += ASCIIShipDestroyed;
                comp.OnEvent += ASCIIUpdateShip;
                comp.OnEvent += ASCIIUpdateShotHit;
                comp.OnEvent += MakeASCII;
                comp.OnEvent += SleepEvent;
            }
            /*if (eventsToFile)
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
            if (!isWritten)
            {
                boardsBaseY = CurrentY + 2;
                isWritten = true;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        [EventFilter(typeof(ShipDestroyedEvent))]
        private void ASCIIShipDestroyed(Event ev)
        {
            ShipDestroyedEvent shipEvent = (ShipDestroyedEvent)ev;
            ASCIIWriteShip(shipEvent.Ship.Owner.ID, shipEvent.Ship, shipEvent.Ship.Length.ToString().ToCharArray()[0], ConsoleColor.White, ConsoleColor.Red);
        }

        [EventFilter(typeof(ShipMovedEvent))]
        private void ASCIIUpdateShip(Event ev)
        {
            ShipMovedEvent shipsEvent = (ShipMovedEvent)ev;
            ASCIIWriteShip(shipsEvent.Ship.Owner.ID, shipsEvent.Ship, ' ', ConsoleColor.White, ConsoleColor.Green);
        }

        [EventFilter(typeof(PlayerShotEvent))]
        private void ASCIIUpdateShot(Event ev)
        {
            PlayerShotEvent shotEvent = (PlayerShotEvent)ev;
            ModifyASCII(shotEvent.Shot.ReceiverPlr.ID, shotEvent.Shot.Coordinates.X, shotEvent.Shot.Coordinates.Y, ' ', ConsoleColor.White, ConsoleColor.DarkGray);
        }

        [EventFilter(typeof(ShipHitEvent))]
        private void ASCIIUpdateShotHit(Event ev)
        {
            ShipHitEvent shipEvent = (ShipHitEvent)ev;
            ModifyASCII(shipEvent.Ship.Owner.ID, shipEvent.HitCoords.X, shipEvent.HitCoords.Y, ' ', ConsoleColor.White, ConsoleColor.Yellow);
        }

        private void ASCIIWriteShip(IDNumber ctrlIdx, Ship ship, char character, ConsoleColor colorText, ConsoleColor colorBack)
        {
            foreach (Coordinates coord in ship.Locations)
            {
                ModifyASCII(ctrlIdx, coord.X, coord.Y, character, colorText, colorBack);
            }
        }

        [EventFilter(typeof(RoundEndEvent))]
        private void CompRoundEnd(Event ev)
        {
            turns = 0;
            roundsRun++;
            WriteScoreRounds();
            Console.ForegroundColor = ConsoleColor.Black;
            AlignToLine(6);
            WriteCenteredText("xxx turns made this round.");
            currentEventString = "!!!New round!!!";
        }

        private void FileWriteEvent()
        {
            fileWriter.WriteLine(currentEventString);
        }

        private void LastEvent(Event ev)
        {
            lastMillis = ev.Millis;
        }

        [EventFilter(typeof(RoundBeginEvent))]
        private void MakeASCII(Event ev)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int x = -1; x < competition.FieldSize.X; x++)
                {
                    for (int y = -1; y < competition.FieldSize.Y; y++)
                    {
                        if (x == -1 || y == -1)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                            if (x == -1)
                            {
                                ModifyASCII(i, -1, y, y.ToString().ToCharArray()[0], ConsoleColor.Gray, ConsoleColor.Black);
                            }
                            else if (y == -1)
                            {
                                ModifyASCII(i, x, -1, x.ToString().ToCharArray()[0], ConsoleColor.Gray, ConsoleColor.Black);
                            }
                        }
                        else
                        {
                            ModifyASCII(i, x, y, ' ', ConsoleColor.White, ConsoleColor.DarkBlue);
                        }
                    }
                }
            }
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

        private void ModifyASCII(int ctrlIdx, int x, int y, char character, ConsoleColor text, ConsoleColor background)
        {
            int sidePad = 10;
            int pad = (Width - 2) - (competition.FieldSize.X + (sidePad * 3));
            int baseX = sidePad + ctrlIdx * (competition.FieldSize.X + pad);
            AlignToCoord(baseX + x, boardsBaseY + y);
            ColorStore prevColors = ColorStore.StoreCurrentColors();
            Console.BackgroundColor = background;
            Console.ForegroundColor = text;
            WriteCharRepeat(character, 1);
            prevColors.Restore();
        }

        [EventFilter(typeof(RoundEndEvent))]
        private void RoundEnd(Event ev)
        {
            turns = 0;
            roundsRun++;
            WriteScoreRounds();
            currentEventString = "!!!New round!!!";
        }

        [EventFilter(typeof(PlayerTurnEndEvent))]
        private void RoundTurn(Event ev)
        {
            if (millisDelay > 0)
            {
                turns++;
                WriteTurns();
                Thread.Sleep(millisDelay);
            }
        }

        [EventFilter(typeof(PlayerLostEvent))]
        private void SleepEvent(Event ev)
        {
            Thread.Sleep(1000);
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
            WriteCenteredText("[" + players[0].Wins + "]     wins     [" + players[1].Wins + "]");
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