using MBC.Core.Events;
using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MBC.Core.Rounds
{
    /// <summary>
    /// The base class for a class deriving from a <see cref="Round"/> that utilizes <see cref="Player"/>s
    /// to cause influence to the <see cref="Event"/>s generated in a <see cref="Round"/>. Provides standardized
    /// functions that most battleship games use.
    /// </summary>
    internal abstract class BattleshipRound : Round
    {
        internal override void HandleEvent(Event ev)
        {
 	        throw new NotImplementedException();
        }
        /// <summary>
        /// Fires the <see cref="RoundBeginEvent"/> and invokes <see cref="Player.NewRound()"/> on all
        /// <see cref="Player"/>s. Picks a random <see cref="Player"/> to have the
        /// current turn.
        /// </summary>
        protected void Begin()
        {
            MakeEvent(new RoundBeginEvent(Registers));

            foreach (var register in Registers)
            {
                register.Ships = new ShipList(MatchConfig.StartingShips);
                register.Shots = new ShotList();
                register.ShotsAgainst = new ShotList();
                try
                {
                    Controllers[register.ID].NewRound();
                }
                catch (ControllerTimeoutException ex)
                {
                    MakeEvent(new PlayerTimeoutEvent(ex));
                }
            }
        }

        /// <summary>
        /// Indicates whether or not the current state of a given <see cref="Register"/> has <see cref="Ship"/>s
        /// that are valid, checking the following:
        /// <list type="bullet">
        /// <item>The <see cref="ShipList"/> is not null</item>
        /// <item>The <see cref="ShipList"/> has the same <see cref="Ship.Length"/>s as the in the <see cref="MatchConfig.StartingShips"/></item>
        /// <item>All <see cref="Ship"/>s in the <see cref="ShipList"/> have been placed</item>
        /// <item>None of the <see cref="Ship"/>s in the <see cref="ShipList"/> are conflicting</item>
        /// </list>
        /// </summary>
        /// <param name="rID">The <see cref="Register"/> to check the ships for.</param>
        /// <returns>A value indicating if all aforementioned conditions are true.</returns>
        protected bool ControllerShipsValid(IDNumber rID)
        {
            return Registers[rID].Ships != null &&
                Registers[rID].Ships.EqualLengthsAs(MatchConfig.StartingShips) &&
                Registers[rID].Ships.ShipsPlaced &&
                Registers[rID].Ships.GetConflictingShips().Count == 0;
        }

        /// <summary>
        /// Indicates whether or not a <see cref="Shot"/> made by a <see cref="Player"/> violates the
        /// standard rules for making a <see cref="Shot"/>.
        /// </summary>
        /// <param name="rID">The <see cref="Player"/> making a <see cref="Shot"/>.</param>
        /// <param name="shot">The <see cref="Shot"/> made by the given <see cref="Player"/>.</param>
        /// <returns>A value indicating if the <see cref="Shot"/> made was invalid.</returns>
        protected bool ControllerShotInvalid(IDNumber rID, Shot shot)
        {
            return shot == null ||
                (shot.Coordinates > MatchConfig.FieldSize) ||
                    (shot.Coordinates < new Coordinates(0, 0)) ||
                    rID == shot.Receiver ||
                    Registers[rID].Shots.Contains(shot) ||
                    !Remaining.Contains(shot.Receiver);
        }

        /// <summary>
        /// For a given <see cref="Register"/>, fires the <see cref="PlayerLostEvent"/>,
        /// calls the <see cref="Player.RoundLost()"/> method in the <see cref="Player"/>,
        /// and removes the <see cref="Player"/> from the remaining <see cref="Register"/>s.
        /// For a given <see cref="Register"/>, fires the <see cref="PlayerLostEvent"/>,
        /// calls the <see cref="ControllerUser.RoundLost()"/> method in the <see cref="ControllerUser"/>,
        /// and removes the <see cref="ControllerUser"/> from the remaining <see cref="Register"/>s.
        /// </summary>
        /// <param name="rID">The <see cref="Player"/> that lost the round.</param>
        protected void MakeLoser(IDNumber rID)
        {
            MakeEvent(new PlayerLostEvent(rID));
            try
            {
                Controllers[rID].RoundLost();
            }
            catch (ControllerTimeoutException ex)
            {
                MakeEvent(new PlayerTimeoutEvent(ex));
            }
        }

        /// <summary>
        /// Finds the next <see cref="Player"/> in line from the currentTurn.
        /// </summary>
        /// <returns>The next <see cref="Player"/> after currentTurn
        /// that remains.</returns>
        protected IDNumber NextRemaining()
        {
            return Remaining[(CurrentTurn + 1) % Remaining.Count];
        }

        /// <summary>
        /// Changes the currentTurn to the next remaining <see cref=Register"/>.
        /// </summary>
        protected void NextTurn()
        {
            MakeEvent(new RoundPlayerActionEvent(CurrentTurn, NextRemaining()));
        }

        /// <summary>
        /// The standard procedure for inquiring a <see cref="ControllerUser"/> for the next <see cref="Shot"/>
        /// to be made.
        /// </summary>
        protected void StandardPlaceShot()
        {
            try
            {
                Shot shotMade = Controllers[CurrentTurn].MakeShot();

                if (ControllerShotInvalid(CurrentTurn, shotMade))
                {
                    MakeEvent(new PlayerShotEvent(CurrentTurn, shotMade));
                    MakeLoser(CurrentTurn);
                    return;
                }
                MakeEvent(new PlayerShotEvent(CurrentTurn, shotMade));
                Controllers[shotMade.Receiver].NotifyOpponentShot(shotMade);

                var shipHit = Registers[shotMade.Receiver].Ships.ShipAt(shotMade.Coordinates);
                var shotHit = shipHit != null;
                if (shotHit)
                {
                    var shipSunk = shipHit.IsSunk(Registers[shotMade.Receiver].ShotsAgainst);

                    MakeEvent(new PlayerHitShipEvent(CurrentTurn, shotMade));
                    Controllers[CurrentTurn].NotifyShotHit(shotMade, shipSunk);
                    if (shipSunk)
                    {
                        MakeEvent(new PlayerShipDestroyedEvent(shotMade.Receiver, shipHit));
                        if (Registers[shotMade.Receiver].ShipsLeft.Count == 0)
                        {
                            MakeLoser(shotMade.Receiver);
                        }
                    }
                }
                else
                {
                    Controllers[CurrentTurn].NotifyShotMiss(shotMade);
                }
            }
            catch (ControllerTimeoutException ex)
            {
                MakeEvent(new PlayerTimeoutEvent(ex));
                MakeLoser(ex.Register.ID);
            }
        }

        /// <summary>
        /// Calls <see cref="Player.PlaceShips()"/> on every <see cref="Player"/> and
        /// creates <see cref="PlayerShipsPlacedEvent"/>s for each. Changes the <see cref="Round.CurrentState"/>
        /// to <see cref="Round.State.Main"/>.
        /// </summary>
        protected void StandardShipPlacement()
        {
            foreach (var rID in Remaining)
            {
                try
                {
                    MakeEvent(new PlayerShipsPlacedEvent(rID, Registers[rID].Ships, Controllers[rID].PlaceShips()));

                    if (!ControllerShipsValid(rID))
                    {
                        MakeLoser(rID);
                    }
                }
                catch (ControllerTimeoutException ex)
                {
                    MakeEvent(new PlayerTimeoutEvent(ex));
                    MakeLoser(rID);
                }
            }
        }

        private static List<Register> RegistersFromControllers(List<Player> controllers)
        {
            var registers = new List<Register>();
            foreach (var controller in controllers)
            {
                registers.Add(controller.Register);
            }
            return registers;
        }
    }
}