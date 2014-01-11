using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MBC.Core.Events;
using MBC.Core.Rounds;
using MBC.Core.Threading;
using MBC.Shared;

namespace MBC.Core.Matches
{
    public class ClassicGame : GameLogic
    {
        private bool shipsPlaced;
        private List<IDNumber> turns;

        public ClassicGame(IDNumber roundID, ActiveMatch match)
            : base(roundID, match)
        {
            FormTeams();
            turns = RandomizeTurns();
            shipsPlaced = false;
            CurrentTurnPlayer = NextTurn();

            AddEventAction(typeof(MatchAddPlayerEvent), PlayerInit);
        }

        public IDNumber CurrentTurnPlayer
        {
            get;
            private set;
        }

        public override bool Ended
        {
            get
            {
                return NextTurn() == CurrentTurnPlayer;
            }
        }

        protected IDNumber DeadTeam
        {
            get;
            private set;
        }

        protected IDNumber FFATeam
        {
            get;
            private set;
        }

        private bool IsRunning
        {
            get;
            set;
        }

        public IDNumber NextTurn()
        {
            int currentTurnIdx;
            for (currentTurnIdx = 0; currentTurnIdx < turns.Count; currentTurnIdx++)
            {
                if (turns[currentTurnIdx] == CurrentTurnPlayer)
                {
                    break;
                }
            }
            for (int i = 1; i < turns.Count; i++)
            {
                IDNumber plr = turns[(currentTurnIdx + i) % turns.Count];
                if (Match.Teams[DeadTeam].Members.Contains(plr)) continue;
                return plr;
            }
            return CurrentTurnPlayer;
        }

        public override void Play()
        {
            if (IsRunning)
            {
                return;
            }
            IsRunning = true;
            while (IsRunning)
            {
                if (Ended)
                {
                    IsRunning = false;
                    return;
                }
                if (!shipsPlaced)
                {
                    ApplyEvent(new RoundBeginEvent(ID));
                    foreach (var ctrl in Match.Controllers)
                    {
                        ctrl.Value.NewRound();
                    }
                    PlaceAllShips();
                    shipsPlaced = true;
                }
                else
                {
                    MakeTurn();
                    CurrentTurnPlayer = NextTurn();
                    if (Ended)
                    {
                        EndLogic();
                    }
                }
            }
        }

        public bool ShipsValid(IDNumber player)
        {
            bool result = Match.Fields[player].Ships != null &&
                Match.Fields[player].Ships.EqualLengthsAs(Match.CompiledConfig.StartingShips) &&
                Match.Fields[player].Ships.ShipsPlaced &&
                Match.Fields[player].Ships.GetConflictingShips().Count == 0;
            foreach (var ship in Match.Fields[player].Ships)
            {
                ship.IsValid(Match.CompiledConfig.FieldSize);
            }

            return result;
        }

        public bool ShotValid(IDNumber shooter, Shot shot)
        {
            return shot != null &&
                shot.Coordinates < Match.CompiledConfig.FieldSize &&
                shot.Coordinates >= new Coordinates(0, 0) &&
                shooter != shot.Receiver &&
                !Match.Fields[shooter].Shots.Contains(shot) &&
                !Match.Teams[DeadTeam].Members.Contains(shot.Receiver);
        }

        public override void Stop()
        {
            IsRunning = false;
        }

        private void EndLogic()
        {
            if (!Match.Teams[DeadTeam].Members.Contains(CurrentTurnPlayer))
            {
                ApplyEvent(new PlayerWonEvent(CurrentTurnPlayer));
                Match.Controllers[CurrentTurnPlayer].RoundWon();
                turns.Remove(CurrentTurnPlayer);
            }
            foreach (var plr in turns)
            {
                ApplyEvent(new PlayerLostEvent(plr));
                Match.Controllers[CurrentTurnPlayer].RoundLost();
            }
            turns.Clear();
            ApplyEvent(new RoundEndEvent(ID));
        }

        private void FormTeams()
        {
            FFATeam = Match.GetTeam("Free-For-All");
            Match.Teams[FFATeam].IsFriendly = false;
            DeadTeam = Match.GetTeam(Constants.TEAM_DEAD_NAME, true);
            foreach (var reg in Match.Registers)
            {
                Match.SetControllerToTeam(reg.Key, FFATeam);
                Match.UnsetControllerFromTeam(reg.Key, DeadTeam);
            }
        }

        private void MakeTurn()
        {
            try
            {
                var shotMade = Match.Controllers[CurrentTurnPlayer].MakeShot();

                if (!ShotValid(CurrentTurnPlayer, shotMade))
                {
                    ApplyEvent(new PlayerShotEvent(CurrentTurnPlayer, shotMade));
                    PlayerLose(CurrentTurnPlayer);
                    return;
                }
                ApplyEvent(new PlayerShotEvent(CurrentTurnPlayer, shotMade));

                Match.Controllers[shotMade.Receiver].OpponentShot(shotMade);

                var shipHit = Match.Fields[shotMade.Receiver].Ships.ShipAt(shotMade.Coordinates);
                if (shipHit != null)
                {
                    var shipSunk = shipHit.IsSunk(Match.Fields[shotMade.Receiver].ShotsAgainst);

                    ApplyEvent(new PlayerHitShipEvent(CurrentTurnPlayer, shotMade));
                    Match.Controllers[CurrentTurnPlayer].ShotHit(shotMade, shipSunk);
                    if (shipSunk)
                    {
                        ApplyEvent(new PlayerShipDestroyedEvent(shotMade.Receiver, shipHit));
                        if (Match.Fields[shotMade.Receiver].ShipsLeft.Count == 0)
                        {
                            PlayerDead(shotMade.Receiver);
                        }
                    }
                }
                else
                {
                    Match.Controllers[CurrentTurnPlayer].ShotMiss(shotMade);
                }
            }
            catch (ControllerTimeoutException ex)
            {
                PlayerTimedOut(CurrentTurnPlayer, ex);
            }
        }

        private void PlaceAllShips()
        {
            foreach (var controller in Match.Controllers)
            {
                try
                {
                    ApplyEvent(new PlayerShipsPlacedEvent(controller.Key, controller.Value.PlaceShips()));

                    if (!ShipsValid(controller.Key))
                    {
                        PlayerLose(controller.Key);
                    }
                }
                catch (ControllerTimeoutException ex)
                {
                    PlayerTimedOut(controller.Key, ex);
                }
            }
        }

        private void PlayerDead(IDNumber plr)
        {
            Match.Teams[DeadTeam].Members.Add(plr);
        }

        private void PlayerInit(Event ev)
        {
            Match.Teams[FFATeam].Members.Add(((MatchAddPlayerEvent)ev).PlayerID);
        }

        private void PlayerLose(IDNumber plr)
        {
            Match.Teams[DeadTeam].Members.Add(plr);
            turns.Remove(plr);
            ApplyEvent(new PlayerLostEvent(plr));
            try
            {
                Match.Controllers[plr].RoundLost();
            }
            catch (ControllerTimeoutException ex)
            {
                ApplyEvent(new PlayerTimeoutEvent(plr, ex));
            }
        }

        private void PlayerTimedOut(IDNumber plr, ControllerTimeoutException ex)
        {
            ApplyEvent(new PlayerTimeoutEvent(plr, ex));
            PlayerLose(plr);
        }

        private List<IDNumber> RandomizeTurns()
        {
            var randomAddList = new List<IDNumber>();
            var result = new List<IDNumber>();
            var randomGenerator = new Random();
            foreach (var reg in Match.Registers)
            {
                randomAddList.Add(reg.Value.ID);
            }
            while (randomAddList.Count > 0)
            {
                var randomResult = randomGenerator.Next(randomAddList.Count);
                result.Add(randomAddList[randomResult]);
                randomAddList.RemoveAt(randomResult);
            }
            return result;
        }
    }
}