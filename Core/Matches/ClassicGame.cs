using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.Core.Events;
using MBC.Core.Rounds;
using MBC.Core.Threading;
using MBC.Shared;

namespace MBC.Core.Matches
{
    public class ClassicGame : GameLogic
    {
        private int currentTurnIdx;
        private List<IDNumber> turns;

        public ClassicGame(IDNumber roundID, ActiveMatch match)
            : base(roundID, match)
        {
            turns = RandomizeTurns();
        }

        public IDNumber CurrentTurnPlayer
        {
            get;
            private set;
        }

        public override bool IsRunning
        {
            get;
            protected set;
        }

        protected IDNumber DeadTeam
        {
            get;
            private set;
        }

        public IDNumber NextTurn()
        {
            for (int i = 1; i < turns.Count; i++)
            {
                IDNumber plr = turns[(currentTurnIdx + i) % turns.Count];
                if (Match.Teams[DeadTeam].Members.Contains(plr)) continue;
                CurrentTurnPlayer = plr;
                return plr;
            }
            throw new InvalidOperationException("No players available for next turn.");
        }

        public override void Play()
        {
            throw new NotImplementedException();
        }

        public bool ShipsValid(IDNumber player)
        {
            return Match.Fields[player].Ships != null &&
                Match.Fields[player].Ships.EqualLengthsAs(Match.CompiledConfig.StartingShips) &&
                Match.Fields[player].Ships.ShipsPlaced &&
                Match.Fields[player].Ships.GetConflictingShips().Count == 0;
        }

        public bool ShotValid(IDNumber shooter, Shot shot)
        {
            return shot != null &&
                shot.Coordinates <= Match.CompiledConfig.FieldSize &&
                shot.Coordinates >= new Coordinates(0, 0) &&
                shooter != shot.Receiver &&
                !Match.Fields[shooter].Shots.Contains(shot) &&
                !Match.Teams[DeadTeam].Members.Contains(shot.Receiver);
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        private void MakeTurn()
        {
            try
            {
                Shot shotMade = Match.Controllers[CurrentTurnPlayer].MakeShot();
                ApplyEvent(new PlayerShotEvent(CurrentTurnPlayer, shotMade));

                if (!ShotValid(CurrentTurnPlayer, shotMade))
                {
                    PlayerLose(CurrentTurnPlayer);
                    return;
                }

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
            catch (MethodTimeoutException ex)
            {
                ApplyEvent(new PlayerTimeoutEvent(CurrentTurnPlayer, ex));
                PlayerLose(CurrentTurnPlayer);
            }
        }

        private void PlayerDead(IDNumber plr)
        {
            Match.Teams[DeadTeam].Members.Add(plr);
        }

        private void PlayerLose(IDNumber plr)
        {
            Match.Teams[DeadTeam].Members.Add(plr);
            ApplyEvent(new PlayerLostEvent(plr));
            try
            {
                Match.Controllers[plr].RoundLost();
            }
            catch (MethodTimeoutException ex)
            {
                ApplyEvent(new PlayerTimeoutEvent(plr, ex));
            }
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
                result.Add(randomResult);
                randomAddList.Remove(randomResult);
            }
            return result;
        }
    }
}