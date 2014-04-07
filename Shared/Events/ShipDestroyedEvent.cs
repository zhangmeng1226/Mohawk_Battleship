using MBC.Shared;
using MBC.Shared.Entities;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/>'s <see cref="Ship"/> that has
    /// been destroyed by another <see cref="Register"/>.
    /// </summary>
    [Serializable]
    public class ShipDestroyedEvent : ShipEvent
    {
        /// <summary>
        /// Constructs this event with the player who owns the destroyed ship.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="destroyedShip"></param>
        public ShipDestroyedEvent(Ship destroyedShip) :
            base(destroyedShip)
        {
        }

        protected internal override void PerformOperation()
        {
            if (!Ship.Active)
            {
                throw new InvalidEventException(this, String.Format("Ship {0} is already destroyed.", Ship));
            }
            Ship.Active = false;
        }
    }
}