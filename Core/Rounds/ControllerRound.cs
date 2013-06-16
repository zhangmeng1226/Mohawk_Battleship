using MBC.Core.Events;
using MBC.Shared;
using System;
using System.Collections.Generic;

namespace MBC.Core
{
    public abstract class ControlledRound : Round
    {
        private List<ControllerUser> controllers;

        public ControlledRound(MatchInfo matchInfo, List<ControllerUser> controllers)
            : base(matchInfo, RegistersFromControllers(controllers))
        {
            this.controllers = controllers;
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
            currentState = State.End;
            MakeEvent(new RoundEndEvent(this));
            base.End();
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
            currentTurn = Registers[randomTurnChooser.Next(Registers.Count)];

            currentState = State.ShipPlacement;
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