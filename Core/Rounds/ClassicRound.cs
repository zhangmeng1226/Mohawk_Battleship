using MBC.Core.Events;
using MBC.Shared;
using System;
using System.Collections.Generic;

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
                foreach (var controller in remainingRegisters)
                {
                    MakeEvent(new ControllerWonEvent(controller));
                }
            }
            base.End();
        }

        /// <summary>
        /// Does the game logic for the current Player turn. Iterates to the next Player in the remaining
        /// list of Player objects. Ends the game when only one Player remains.
        /// </summary>
        protected internal override void Main()
        {
            var next = NextRemaining();
            if (currentTurn == null || next == null)
            {
                //Game is over
                End();
                return;
            }

            var receiver = next;

            var shotsFromSender = currentTurn.Shots;

            try
            {
                Shot shot = controllers[currentTurn.ID].MakeShot();
                MakeEvent(new ControllerShotEvent(currentTurn, shot));

                if (shot == null || ShotOutOfBounds(shot) || ShotSuicidal(shot) || ShotRepeated(shot) || ShotDestroyed(shot))
                {
                    //The current controller violated one of the rules of the round.

                    MakeLoser(currentTurn);
                    NextTurn();
                    return;
                }
                currentTurn.Shots.Add(shot);

                //Get the actual receiver of the shot
                receiver = registers[shot.Receiver];

                Ship shotShip = receiver.Ships.ShipAt(shot.Coordinates);
                if (shotShip != null)
                {
                    //The shot made by the sender hit a ship.

                    MakeEvent(new ControllerHitShipEvent(currentTurn, receiver, shot));
                    bool sunk = shotShip.IsSunk(currentTurn.Shots.ShotsToReceiver(shot.Receiver));
                    controllers[currentTurn.ID].NotifyShotHit(shot, sunk);
                    if (sunk)
                    {
                        //The last shot sunk a receiver's ship.

                        MakeEvent(new ControllerDestroyedShipEvent(currentTurn, receiver, shotShip));
                        if (receiver.Ships.GetSunkShips(currentTurn.Shots.ShotsToReceiver(receiver.ID)).Count == receiver.Ships.Count)
                        {
                            //All of the receiving controller's ships have been destroyed.
                            Console.WriteLine("Loser " + receiver);
                            MakeLoser(receiver);
                        }
                    }
                }
                else
                {
                    controllers[currentTurn.ID].NotifyShotMiss(shot);
                }

                if (remainingRegisters.Contains(receiver))
                {
                    //The receiver is still in the round.

                    controllers[receiver.ID].NotifyOpponentShot(shot);
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
                    (shot.Coordinates < new Coordinates(0, 0));
        }

        private bool ShotSuicidal(Shot shot)
        {
            return currentTurn.ID == shot.Receiver;
        }

        private bool ShotRepeated(Shot shot)
        {
            return registers[currentTurn.ID].Shots.Contains(shot);
        }

        private bool ShotDestroyed(Shot shot)
        {
            return !remainingRegisters.Contains(registers[shot.Receiver]);
        }
    }
}