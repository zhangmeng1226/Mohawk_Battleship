using MBC.App.Terminal.Layouts;
using MBC.App.Terminal.Modules;
using MBC.Core;
using MBC.Core.Events;
using MBC.Core.Matches;
using MBC.Core.Threading;
using MBC.Shared;
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
        private ActiveMatch competition;
        private string currentEventString;
        private StreamWriter fileWriter;
        private bool isWritten;
        private int lastMillis;
        private int millisDelay;
        private int roundsRun;
        private FuncThreader threader;
        private int turns;

        public CompetitionRun(ActiveMatch comp, int delay, bool eventsToFile)
        {
            isWritten = false;
            boardsBaseY = -1;
            turns = 0;
            roundsRun = 0;
            competition = comp;
            currentEventString = "";
            millisDelay = delay;
            competition.AddEventAction(typeof(PlayerShotEvent), CompRoundTurn);
            competition.AddEventAction(typeof(RoundEndEvent), CompRoundEnd);
            competition.AddEventAction(typeof(PlayerShipDestroyedEvent), ShipDestroyed);
            competition.AddEventAction(typeof(PlayerShotEvent), PlayerShot);
            competition.AddEventAction(typeof(PlayerHitShipEvent), PlayerHit);
            competition.AddEventAction(typeof(MatchEndEvent), MatchEnd);
            competition.AddEventAction(typeof(Event), LastEvent);
            if (millisDelay > 0)
            {
                competition.AddEventAction(typeof(PlayerShipDestroyedEvent), ASCIIShipDestroyed);
                competition.AddEventAction(typeof(PlayerShotEvent), ASCIIUpdateShot);
                competition.AddEventAction(typeof(PlayerHitShipEvent), ASCIIUpdateShotHit);
                competition.AddEventAction(typeof(PlayerShipsPlacedEvent), ASCIIUpdateShips);
                competition.AddEventAction(typeof(RoundBeginEvent), MakeASCII);
            }
            threader = new FuncThreader();
            if (eventsToFile)
            {
                fileWriter = new StreamWriter(Environment.CurrentDirectory + "\\..\\match_" + competition.ID + ".txt");
                fileWriter.WriteLine("START OF MATCH " + competition.ID);
            }
        }

        public void Begin()
        {
            threader.RunMethod(new Action(competition.Play));
        }

        protected override void Display()
        {
            WriteHeader();
            WriteScoreRounds();
            WriteTurns();
            WriteCurrentEvent();
            if (!isWritten)
            {
                boardsBaseY = CurrentY + 2;
                isWritten = true;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void ASCIIShipDestroyed(Event ev)
        {
            PlayerShipDestroyedEvent shipEvent = (PlayerShipDestroyedEvent)ev;
            ASCIIWriteShip(shipEvent.Player, shipEvent.DestroyedShip, shipEvent.DestroyedShip.Length.ToString().ToCharArray()[0], ConsoleColor.White, ConsoleColor.Red);
        }

        private void ASCIIUpdateShips(Event ev)
        {
            PlayerShipsPlacedEvent shipsEvent = (PlayerShipsPlacedEvent)ev;
            foreach (Ship ship in competition.Fields[shipsEvent.Player].ShipsLeft)
            {
                ASCIIWriteShip(shipsEvent.Player, ship, ' ', ConsoleColor.White, ConsoleColor.Green);
                AlignToCoord(4, 4);
                WriteText("Wrote ship ", ship);
                Thread.Sleep(1000);
            }
        }

        private void ASCIIUpdateShot(Event ev)
        {
            PlayerShotEvent shotEvent = (PlayerShotEvent)ev;
            ModifyASCII(shotEvent.Shot.Receiver, shotEvent.Shot.Coordinates.X, shotEvent.Shot.Coordinates.Y, ' ', ConsoleColor.White, ConsoleColor.DarkGray);
        }

        private void ASCIIUpdateShotHit(Event ev)
        {
            PlayerHitShipEvent shipEvent = (PlayerHitShipEvent)ev;
            ModifyASCII(shipEvent.HitShot.Receiver, shipEvent.HitShot.Coordinates.X, shipEvent.HitShot.Coordinates.Y, ' ', ConsoleColor.White, ConsoleColor.Yellow);
        }

        private void ASCIIWriteShip(IDNumber ctrlIdx, Ship ship, char character, ConsoleColor colorText, ConsoleColor colorBack)
        {
            for (int i = 0; i < ship.Length; i++)
            {
                ModifyASCII(ctrlIdx, ship.Location.X + (ship.Orientation == ShipOrientation.Horizontal ? i : 0),
                    ship.Location.Y + (ship.Orientation == ShipOrientation.Vertical ? i : 0),
                    character, colorText, colorBack);
            }
        }

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

        private void CompRoundTurn(Event ev)
        {
            turns++;
            WriteTurns();
        }

        private void FileWriteEvent()
        {
            fileWriter.WriteLine(currentEventString);
        }

        private void LastEvent(Event ev)
        {
            lastMillis = ev.Millis;
        }

        private void MakeASCII(Event ev)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int x = -1; x < competition.CompiledConfig.FieldSize.X; x++)
                {
                    for (int y = -1; y < competition.CompiledConfig.FieldSize.Y; y++)
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
            int pad = (Width - 2) - (competition.CompiledConfig.FieldSize.X + (sidePad * 3));
            int baseX = sidePad + ctrlIdx * (competition.CompiledConfig.FieldSize.X + pad);
            AlignToCoord(baseX + x, boardsBaseY + y);
            ColorStore prevColors = ColorStore.StoreCurrentColors();
            Console.BackgroundColor = background;
            Console.ForegroundColor = text;
            WriteCharRepeat(character, 1);
            prevColors.Restore();
        }

        private void PlayerHit(Event ev)
        {
            PlayerHitShipEvent hit = (PlayerHitShipEvent)ev;
            currentEventString = string.Format("[{4}ms] {0} hit {1}'s ship ({2}) at {3}",
                competition.Registers[hit.Player],
                competition.Registers[hit.HitShot.Receiver],
                competition.Fields[hit.HitShot.Receiver].Ships.ShipAt(hit.HitShot.Coordinates),
                hit.HitShot.Coordinates,
                hit.Millis - lastMillis);
            WriteCurrentEvent();
        }

        private void PlayerShot(Event ev)
        {
            PlayerShotEvent shot = (PlayerShotEvent)ev;
            currentEventString = string.Format("[{3}ms] {0} made a shot at {1} at coords {2}",
                competition.Registers[shot.Player],
                competition.Registers[shot.Shot.Receiver],
                shot.Shot.Coordinates,
                shot.Millis - lastMillis);
            WriteCurrentEvent();
        }

        private void ShipDestroyed(Event ev)
        {
            PlayerShipDestroyedEvent destroyed = (PlayerShipDestroyedEvent)ev;
            currentEventString = string.Format("[{2}ms] {0}'s ship ({1}) is destroyed",
                competition.Registers[destroyed.Player],
                destroyed.DestroyedShip,
                destroyed.Millis - lastMillis);
            WriteCurrentEvent();
        }

        private void WriteCurrentEvent()
        {
            if (millisDelay > 0)
            {
                /*Console.ForegroundColor = ConsoleColor.Green;
                for (int i = 0; i < 4; i++)
                {
                    AlignToLine(8 + i);
                    WriteCharRepeat(' ', currentEventString.Length);
                }
                //AlignToLine(8);
                //WriteCenteredText(currentEventString);*/
                if (fileWriter != null)
                {
                    FileWriteEvent();
                }
                Thread.Sleep(millisDelay);
            }
        }

        private void WriteHeader()
        {
            AlignToLine(0);
            Console.ForegroundColor = ConsoleColor.White;
            WriteCenteredText("=====COMPETITION IN PROGRESS=====");
            AlignToLine(2);
            WriteCenteredText(competition.Registers[0] + " vs. " + competition.Registers[1]);
        }

        private void WriteScoreRounds()
        {
            Console.ForegroundColor = ConsoleColor.White;
            AlignToLine(3);
            WriteCenteredText("[" + competition.Registers[0].Score + "]     score     [" + competition.Registers[1].Score + "]");
            NewLine();
            WriteCenteredText(roundsRun + " rounds out of " + competition.CompiledConfig.NumberOfRounds);
        }

        private void WriteTurns()
        {
            AlignToLine(6);
            Console.ForegroundColor = ConsoleColor.Red;
            WriteCenteredText(turns + " turns made this round.");
        }
    }
}