using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="ControllerRegister"/>'s <see cref="Ship"/> that has
    /// been destroyed by another <see cref="ControllerRegister"/>.
    /// </summary>
    public class ControllerDestroyedShipEvent : ControllerEvent
    {
        private Ship destroyed;
        private ControllerRegister owner;

        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor, stores the other parameters,
        /// and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="register">The <see cref="ControllerRegister"/> that destroyed the <see cref="Ship"/>.</param>
        /// <param name="shipOwner">The <see cref="ControllerRegister"/> that owns the destroyed <see cref="Ship"/></param>
        /// <param name="destroyedShip">The destroyed <see cref="Ship"/>.</param>
        public ControllerDestroyedShipEvent(ControllerRegister register, ControllerRegister shipOwner, Ship destroyedShip)
            : base(register)
        {
            this.destroyed = destroyedShip;
            this.owner = shipOwner;

            message = register + " destroyed a ship at " + destroyedShip + " from " + shipOwner + ".";
        }

        /// <summary>
        /// Gets the <see cref="Ship"/> that was destroyed.
        /// </summary>
        public Ship DestroyedShip
        {
            get
            {
                return destroyed;
            }
        }

        /// <summary>
        /// Gets the <see cref="ControllerRegister"/> owning the destroyed <see cref="Ship"/>.
        /// </summary>
        public ControllerRegister ShipOwner
        {
            get
            {
                return owner;
            }
        }
    }
}