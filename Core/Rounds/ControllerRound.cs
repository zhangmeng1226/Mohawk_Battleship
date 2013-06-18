using MBC.Core.Events;
using MBC.Shared;
using System;
using System.Collections.Generic;

namespace MBC.Core
{
    public abstract class ControlledRound : Round
    {
        /// <summary>
        ///
        /// </summary>
        protected ControllerRegister currentTurn;

        protected List<ControllerRegister> remainingRegisters;

        private List<ControllerUser> controllers;

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

        public int RemainingCount
        {
            get
            {
                return RemainingRegisters.Count;
            }
        }

        public List<ControllerRegister> RemainingRegisters
        {
            get
            {
                return remainingRegisters;
            }
        }

        protected List<ControllerUser> Controllers
        {
            get
            {
                return controllers;
            }
        }

        public override void End()
        {
            MakeEvent(new RoundEndEvent(this));
            base.End();
        }

        protected override void Begin()
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
        /// Determines if the ships associated with the given Entity are placed, and do not conflict with each other.
        /// </summary>
        /// <param name="register">The Entity ships to check</param>
        /// <returns>true if the ships related to the given Entity are valid, false otherwise.</returns>
        protected bool ControllerShipsValid(ControllerRegister register)
        {
            return register.Ships != null && register.Ships.ShipsPlaced && register.Ships.GetConflictingShips().Count == 0;
        }

        protected void MakeLoser(ControllerRegister loser)
        {
            MakeEvent(new ControllerLostEvent(loser));
            try
            {
                Controllers[loser.ID].RoundLost();
            }
            catch (ControllerTimeoutException ex)
            {
                MakeEvent(new ControllerTimeoutEvent(ex));
            }
            RemainingRegisters.Remove(loser);
        }

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
        /// Switches the turn from the current Player to the next Player. The current Player must exist in the
        /// remaining Player list or this method will throw an exception.
        /// </summary>
        /// <exception cref="InvalidOperationException">The current player had been removed the existing player list before switching turns.</exception>
        protected void NextTurn()
        {
            currentTurn = NextRemaining();
        }
        protected override void ShipPlacement()
        {
            foreach (var register in Registers)
            {
                try
                {
                    Controllers[register.ID].PlaceShips();
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