using MBC.Core.Events;
using MBC.Shared;
using System;
using System.Collections.Generic;

namespace MBC.Core
{
    /// <summary>
    /// The base class for a class deriving from a <see cref="Round"/> that utilizes <see cref="ControllerUser"/>s
    /// to cause influence to the <see cref="Event"/>s generated in a <see cref="Round"/>. Provides standardized
    /// functions that most battleship games use.
    /// </summary>
    internal abstract class ControlledRound : Round
    {
        /// <summary>
        /// A list of <see cref="ControllerUser"/>s involved in influencing generated <see cref="Event"/>s.
        /// </summary>
        protected List<ControllerUser> controllers;

        /// <summary>
        /// The <see cref="ControllerUser"/> that has the current turn.
        /// </summary>
        protected ControllerRegister currentTurn;

        /// <summary>
        /// The <see cref="ControllerUser"/>s that have not been defeated.
        /// </summary>
        protected List<ControllerRegister> remainingRegisters;

        /// <summary>
        /// Attaches the <see cref="MatchInfo"/> provided by a <see cref="Match"/> and retrieves the list of
        /// <see cref="ControllerUser"/>s that persist in a <see cref="Match"/>.
        /// </summary>
        /// <param name="matchInfo">The <see cref="MatchInfo"/> from a round to associate with.</param>
        /// <param name="controllers">The <see cref="ControllerUser"/>s to utilize.</param>
        public ControlledRound(MatchInfo matchInfo, List<ControllerUser> controllers)
            : base(matchInfo, RegistersFromControllers(controllers))
        {
            this.controllers = controllers;
            remainingRegisters = new List<ControllerRegister>();
            foreach (var register in registers)
            {
                remainingRegisters.Add(register);
            }
        }

        /// <summary>
        /// Fires the <see cref="RoundBeginEvent"/> and invokes <see cref="ControllerUser.NewRound()"/> on all
        /// <see cref="ControllerUser"/>s. Picks a random <see cref="ControllerUser"/> to have the
        /// current turn.
        /// </summary>
        protected internal override void Begin()
        {
            MakeEvent(new RoundBeginEvent(this));

            foreach (var controller in controllers)
            {
                try
                {
                    controller.NewRound();
                }
                catch (ControllerTimeoutException ex)
                {
                    MakeEvent(new ControllerTimeoutEvent(ex));
                }
            }

            var randomTurnChooser = new Random();
            currentTurn = registers[randomTurnChooser.Next(registers.Count)];

            currentState = State.ShipPlacement;
        }

        /// <summary>
        /// Indicates whether or not the current state of a given <see cref="ControllerRegister"/> has <see cref="Ship"/>s
        /// that are valid, checking the following:
        /// <list type="bullet">
        /// <item>The <see cref="ShipList"/> is not null</item>
        /// <item>All <see cref="Ship"/>s in the <see cref="ShipList"/> have been placed</item>
        /// <item>None of the <see cref="Ship"/>s in the <see cref="ShipList"/> are conflicting</item>
        /// </list>
        /// </summary>
        /// <param name="register">The <see cref="ControllerRegister"/> to check the ships for.</param>
        /// <returns>A value indicating if all aforementioned conditions are true.</returns>
        protected bool ControllerShipsValid(ControllerRegister register)
        {
            return register.Ships != null && register.Ships.ShipsPlaced && register.Ships.GetConflictingShips().Count == 0;
        }

        /// <summary>
        /// Indicates whether or not a <see cref="Shot"/> made by a <see cref="ControllerUser"/> violates the
        /// standard rules for making a <see cref="Shot"/>.
        /// </summary>
        /// <param name="register">The <see cref="ControllerUser"/> making a <see cref="Shot"/>.</param>
        /// <param name="shot">The <see cref="Shot"/> made by the given <see cref="ControllerUser"/>.</param>
        /// <returns>A value indicating if the <see cref="Shot"/> made was invalid.</returns>
        protected bool ControllerShotInvalid(ControllerRegister register, Shot shot)
        {
            return shot == null ||
                (shot.Coordinates > MatchInfo.FieldSize) ||
                    (shot.Coordinates < new Coordinates(0, 0)) ||
                    register.ID == shot.Receiver ||
                    register.Shots.Contains(shot) ||
                    !remainingRegisters.Contains(registers[shot.Receiver]);
        }

        /// <summary>
        /// For a given <see cref="ControllerRegister"/>, fires the <see cref="ControllerLostEvent"/>,
        /// calls the <see cref="ControllerUser.RoundLost()"/> method in the <see cref="ControllerUser"/>,
        /// and removes the <see cref="ControllerUser"/> from the remaining <see cref="ControllerRegister"/>s.
        /// </summary>
        /// <param name="loser">The <see cref="ControllerUser"/> that lost the round.</param>
        protected void MakeLoser(ControllerRegister loser)
        {
            MakeEvent(new ControllerLostEvent(loser));
            try
            {
                controllers[loser.ID].RoundLost();
            }
            catch (ControllerTimeoutException ex)
            {
                MakeEvent(new ControllerTimeoutEvent(ex));
            }
            remainingRegisters.Remove(loser);
        }

        /// <summary>
        /// Finds the next <see cref="ControllerUser"/> in line from the currentTurn.
        /// </summary>
        /// <returns>The next <see cref="ControllerUser"/> after currentTurn
        /// that remains.</returns>
        protected ControllerRegister NextRemaining()
        {
            if (currentTurn == null)
            {
                return null;
            }

            var next = (currentTurn.ID + 1) % registers.Count;
            while (next != currentTurn.ID)
            {
                foreach (var register in remainingRegisters)
                {
                    if (register.ID == next)
                    {
                        return register;
                    }
                }
                next = (next + 1) % registers.Count;
            }
            return null;
        }

        /// <summary>
        /// Changes the currentTurn to the next remaining <see cref="ControllerRegister"/>.
        /// </summary>
        protected void NextTurn()
        {
            currentTurn = NextRemaining();
        }

        /// <summary>
        /// Calls <see cref="ControllerUser.PlaceShips()"/> on every <see cref="ControllerUser"/> and
        /// creates <see cref="ControllerShipsPlacedEvent"/>s for each. Changes the <see cref="Round.CurrentState"/>
        /// to <see cref="Round.State.Main"/>.
        /// </summary>
        protected override void ShipPlacement()
        {
            foreach (var register in Registers)
            {
                try
                {
                    controllers[register.ID].PlaceShips();
                    MakeEvent(new ControllerShipsPlacedEvent(register, new ShipList(register.Ships)));

                    if (!ControllerShipsValid(register))
                    {
                        MakeLoser(register);
                    }
                }
                catch (ControllerTimeoutException ex)
                {
                    MakeEvent(new ControllerTimeoutEvent(ex));
                    MakeLoser(register);
                }
            }
            currentState = State.Main;
        }

        private static List<ControllerRegister> RegistersFromControllers(List<ControllerUser> controllers)
        {
            var registers = new List<ControllerRegister>();
            foreach (var controller in controllers)
            {
                registers.Add(controller.Register);
            }
            return registers;
        }
    }
}