using MBC.Core.Events;
using MBC.Shared;
using System;
using System.Collections.Generic;

namespace MBC.Core
{
    /// <summary>
    /// A derivative of the <see cref="ControlledRound"/> which provides a turn-by-turn based game of
    /// battleship.
    /// </summary>
    internal class ClassicRound : ControlledRound
    {
        /// <summary>
        /// Passes parameters to the base constructor.
        /// </summary>
        /// <param name="info">The <see cref="MatchInfo"/> from a round to associate with.</param>
        /// <param name="controllers">The <see cref="ControllerUser"/>s to utilize.</param>
        public ClassicRound(MatchInfo info, List<ControllerUser> controllers)
            : base(info, controllers) { }

        /// <summary>
        /// Creates a new <see cref="ControllerWonEvent"/> for the <see cref="ControllerUser"/>(s) that won
        /// and calls the base class <see cref="Round.End()"/>.
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
        /// Performs a step in the game logic for the battleship game.
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

                if (ControllerShotInvalid(currentTurn, shot))
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
    }
}