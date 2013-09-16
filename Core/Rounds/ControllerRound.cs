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
    internal abstract class ControlledRound : Round
    {
        /// <summary>
        /// A list of <see cref="Player"/>s involved in influencing generated <see cref="Event"/>s.
        /// </summary>
        [XmlIgnore]
        protected List<Player> Controllers;

        /// <summary>
        /// Attaches the <see cref="MatchInfo"/> provided by a <see cref="Match"/> and retrieves the list of
        /// <see cref="Player"/>s that persist in a <see cref="Match"/>.
        /// </summary>
        /// <param name="matchInfo">The <see cref="MatchInfo"/> from a round to associate with.</param>
        /// <param name="controllers">The <see cref="Player"/>s to utilize.</param>
        public ControlledRound(MatchInfo matchInfo, List<Player> controllers)
            : base(matchInfo, RegistersFromControllers(controllers))
        {
            Controllers = controllers;
        }

        /// <summary>
        /// Implements a standard <see cref="Round"/> ending.
        /// </summary>
        public override void End()
        {
            foreach (var rID in Remaining)
            {
                try
                {
                    Controllers[rID].RoundWon();
                }
                catch (ControllerTimeoutException ex)
                {
                    MakeEvent(new ControllerTimeoutEvent(ex));
                }
            }
            base.End();
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
                register.Ships = new ShipList(MatchInfo.StartingShips);
                register.Shots = new ShotList();
                register.ShotsAgainst = new ShotList();
                try
                {
                    Controllers[register.ID].NewRound();
                }
                catch (ControllerTimeoutException ex)
                {
                    MakeEvent(new ControllerTimeoutEvent(ex));
                }
            }
        }

        /// <summary>
        /// Indicates whether or not the current state of a given <see cref="Register"/> has <see cref="Ship"/>s
        /// that are valid, checking the following:
        /// <list type="bullet">
        /// <item>The <see cref="ShipList"/> is not null</item>
        /// <item>The <see cref="ShipList"/> has the same <see cref="Ship.Length"/>s as the in the <see cref="MatchInfo.StartingShips"/></item>
        /// <item>All <see cref="Ship"/>s in the <see cref="ShipList"/> have been placed</item>
        /// <item>None of the <see cref="Ship"/>s in the <see cref="ShipList"/> are conflicting</item>
        /// </list>
        /// </summary>
        /// <param name="rID">The <see cref="Register"/> to check the ships for.</param>
        /// <returns>A value indicating if all aforementioned conditions are true.</returns>
        protected bool ControllerShipsValid(ControllerID rID)
        {
            return Registers[rID].Ships != null &&
                Registers[rID].Ships.EqualLengthsAs(MatchInfo.StartingShips) &&
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
        protected bool ControllerShotInvalid(ControllerID rID, Shot shot)
        {
            return shot == null ||
                (shot.Coordinates > MatchInfo.FieldSize) ||
                    (shot.Coordinates < new Coordinates(0, 0)) ||
                    rID == shot.Receiver ||
                    Registers[rID].Shots.Contains(shot) ||
                    !Remaining.Contains(shot.Receiver);
        }

        /// <summary>
<<<<<<< HEAD
        /// For a given <see cref="ControllerRegister"/>, fires the <see cref="ControllerLostEvent"/>,
        /// calls the <see cref="Player.RoundLost()"/> method in the <see cref="Player"/>,
        /// and removes the <see cref="Player"/> from the remaining <see cref="ControllerRegister"/>s.
=======
        /// For a given <see cref="Register"/>, fires the <see cref="ControllerLostEvent"/>,
        /// calls the <see cref="ControllerUser.RoundLost()"/> method in the <see cref="ControllerUser"/>,
        /// and removes the <see cref="ControllerUser"/> from the remaining <see cref="Register"/>s.
>>>>>>> origin/master
        /// </summary>
        /// <param name="rID">The <see cref="Player"/> that lost the round.</param>
        protected void MakeLoser(ControllerID rID)
        {
            MakeEvent(new ControllerLostEvent(rID));
            try
            {
                Controllers[rID].RoundLost();
            }
            catch (ControllerTimeoutException ex)
            {
                MakeEvent(new ControllerTimeoutEvent(ex));
            }
        }

        /// <summary>
        /// Finds the next <see cref="Player"/> in line from the currentTurn.
        /// </summary>
        /// <returns>The next <see cref="Player"/> after currentTurn
        /// that remains.</returns>
        protected ControllerID NextRemaining()
        {
            return Remaining[(CurrentTurn + 1) % Remaining.Count];
        }

        /// <summary>
        /// Changes the currentTurn to the next remaining <see cref=Register"/>.
        /// </summary>
        protected void NextTurn()
        {
            MakeEvent(new RoundTurnChangeEvent(CurrentTurn, NextRemaining()));
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
                    MakeEvent(new ControllerShotEvent(CurrentTurn, shotMade));
                    MakeLoser(CurrentTurn);
                    return;
                }
                MakeEvent(new ControllerShotEvent(CurrentTurn, shotMade));
                Controllers[shotMade.Receiver].NotifyOpponentShot(shotMade);

                var shipHit = Registers[shotMade.Receiver].Ships.ShipAt(shotMade.Coordinates);
                var shotHit = shipHit != null;
                if (shotHit)
                {
                    var shipSunk = shipHit.IsSunk(Registers[shotMade.Receiver].ShotsAgainst);

                    MakeEvent(new ControllerHitShipEvent(CurrentTurn, shotMade));
                    Controllers[CurrentTurn].NotifyShotHit(shotMade, shipSunk);
                    if (shipSunk)
                    {
                        MakeEvent(new ControllerShipDestroyedEvent(shotMade.Receiver, shipHit));
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
                MakeEvent(new ControllerTimeoutEvent(ex));
                MakeLoser(ex.Register.ID);
            }
        }

        /// <summary>
        /// Calls <see cref="Player.PlaceShips()"/> on every <see cref="Player"/> and
        /// creates <see cref="ControllerShipsPlacedEvent"/>s for each. Changes the <see cref="Round.CurrentState"/>
        /// to <see cref="Round.State.Main"/>.
        /// </summary>
        protected void StandardShipPlacement()
        {
            foreach (var rID in Remaining)
            {
                try
                {
                    MakeEvent(new ControllerShipsPlacedEvent(rID, Registers[rID].Ships, Controllers[rID].PlaceShips()));

                    if (!ControllerShipsValid(rID))
                    {
                        MakeLoser(rID);
                    }
                }
                catch (ControllerTimeoutException ex)
                {
                    MakeEvent(new ControllerTimeoutEvent(ex));
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