using MBC.Core.Rounds;
using MBC.Shared;
using System;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="ControllerRegister"/>'s <see cref="Ship"/> that has
    /// been destroyed by another <see cref="ControllerRegister"/>.
    /// </summary>
    public class ControllerShipDestroyedEvent : ControllerEvent
    {
        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor, stores the other parameters,
        /// and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="register">The <see cref="ControllerRegister"/> that destroyed the <see cref="Ship"/>.</param>
        /// <param name="shipOwner">The <see cref="ControllerRegister"/> that owns the destroyed <see cref="Ship"/></param>
        /// <param name="destroyedShip">The destroyed <see cref="Ship"/>.</param>
        public ControllerShipDestroyedEvent(ControllerID register, Ship destroyedShip)
            : base(register)
        {
            DestroyedShip = destroyedShip;
        }

        protected internal override void GenerateMessage()
        {
            Message = RegisterID + " has had their ship at " + DestroyedShip + " destroyed.";
        }

        /// <summary>
        /// Gets the <see cref="Ship"/> that was destroyed.
        /// </summary>
        public Ship DestroyedShip
        {
            get;
            private set;
        }

        internal override void ProcBackward(Round round)
        {
            round.Registers[RegisterID].ShipsLeft.Add(DestroyedShip);
        }

        internal override void ProcForward(Round round)
        {
            round.Registers[RegisterID].ShipsLeft.Remove(DestroyedShip);
        }
    }
}