using MBC.Core.Events;
using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core
{
    public class ClassicRound : ControlledRound
    {
        public ClassicRound(MatchInfo info, List<ControllerUser> controllers)
            : base(info, controllers) { }

        /// <summary>
        /// Ends the Round. Fires ControllerWonEvent and ControllerLostEvent depending on the existance of remaning
        /// Player objects.
        /// </summary>
        public override void End()
        {
            if (currentTurn == null)
            {
                //There is a winner (or every controller is a loser).
                foreach (var controller in Registers)
                {
                    if (RemainingRegisters.Contains(controller))
                    {
                        MakeEvent(new ControllerWonEvent(controller));
                    }
                    else
                    {
                        MakeEvent(new ControllerLostEvent(controller));
                    }
                }
            }
            base.End();
        }

        /// <summary>
        /// Does the game logic for the current Player turn. Iterates to the next Player in the remaining
        /// list of Player objects. Ends the game when only one Player remains.
        /// </summary>
        protected override void DoMainLogic()
        {
            if (currentTurn == null)
            {
                //Game is over
                End();
                return;
            }

            var receiver = NextRemaining();

            var shotsFromSender = currentTurn.Shots;

            try
            {
                Shot shot = Controllers[currentTurn.ID].MakeShot(receiver.ID);
                MakeEvent(new ControllerShotEvent(currentTurn, receiver, shot));
                currentTurn.Shots.Add(shot);

                if (ShotOutOfBounds(shot) || ShotSuicidal(shot) || ShotRepeated(shot) || ShotDestroyed(shot))
                {
                    //The current controller violated one of the rules of the round.

                    MakeLoser(currentTurn);
                    NextTurn();
                    return;
                }

                //Get the actual receiver of the shot
                receiver = Registers[shot.Receiver];

                Ship shotShip = receiver.Ships.ShipAt(shot.Coordinates);
                if (shotShip != null)
                {
                    //The shot made by the sender hit a ship.

                    MakeEvent(new ControllerHitShipEvent(currentTurn, receiver, shot));
                    bool sunk = shotShip.IsSunk(currentTurn.Shots.ShotsToReceiver(shot.Receiver));
                    Controllers[currentTurn.ID].NotifyShotHit(shot, sunk);
                    if (sunk)
                    {
                        //The last shot sunk a receiver's ship.

                        MakeEvent(new ControllerDestroyedShipEvent(currentTurn, receiver, shotShip));
                        if (receiver.Ships.GetSunkShips(currentTurn.Shots.ShotsToReceiver(receiver.ID)).Count == receiver.Ships.Count)
                        {
                            //All of the receiving controller's ships have been destroyed.
                            MakeLoser(receiver);
                        }
                    }
                }
                else
                {
                    Controllers[currentTurn.ID].NotifyShotMiss(shot);
                }

                if (RemainingRegisters.Contains(receiver))
                {
                    //The receiver is still in the round.

                    Controllers[receiver.ID].NotifyOpponentShot(shot);
                }
            }
            catch (ControllerTimeoutException ex)
            {
                //An aforementioned controller timed out.

                MakeEvent(new ControllerTimeoutEvent(ex));
                MakeLoser(ex.Register);
            }

            NextTurn();
        }

        private bool ShotOutOfBounds(Shot shot)
        {
            return (shot.Coordinates > MatchInfo.FieldSize) ||
                    (shot.Coordinates < MatchInfo.FieldSize);
        }

        private bool ShotSuicidal(Shot shot)
        {
            return currentTurn.ID == shot.Receiver;
        }

        private bool ShotRepeated(Shot shot)
        {
            return Registers[currentTurn.ID].Shots.Contains(shot);
        }

        private bool ShotDestroyed(Shot shot)
        {
            return !RemainingRegisters.Contains(Registers[shot.Receiver]);
        }
    }
}
