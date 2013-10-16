using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private string currentEventString;
        private StreamWriter fileWriter;
        private int lastMillis;
        private int millisDelay;
        private int roundsRun;
        private FuncThreader threader;
        private int turns;

        public CompetitionRun(ActiveMatch comp, int delay, bool eventsToFile)
        {
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
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void CompRoundEnd(Event ev)
        {
            turns = 0;
            roundsRun++;
            WriteScoreRounds();
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
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < 4; i++)
            {
                AlignToLine(8 + i);
                WriteCharRepeat(' ', currentEventString.Length);
            }
            AlignToLine(8);
            WriteCenteredText(currentEventString);
            if (fileWriter != null)
            {
                FileWriteEvent();
            }
            if (millisDelay > 0)
            {
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