using MBC.Shared;

namespace MBC.Core.Events
{
    public class ControllerDestroyedShipEvent : ControllerEvent
    {
        private Ship destroyed;
        private ControllerRegister owner;

        public ControllerDestroyedShipEvent(ControllerRegister register, ControllerRegister shipOwner, Ship destroyedShip)
            : base(register)
        {
            this.destroyed = destroyedShip;
            this.owner = shipOwner;

            message = register + " destroyed a ship at " + destroyedShip + " from " + shipOwner + ".";
        }
    }
}