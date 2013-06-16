using MBC.Shared;

namespace MBC.Core.Events
{
    public class ControllerHitShipEvent : ControllerEvent
    {
        private Shot coords;
        private ControllerRegister opponent;

        public ControllerHitShipEvent(ControllerRegister register, ControllerRegister opposer, Shot shot)
            : base(register)
        {
            this.coords = shot;
            this.opponent = opposer;

            message = register + " hit a " + opposer + " ship at " + shot.Coordinates;
        }

        public Shot HitCoords
        {
            get
            {
                return coords;
            }
        }

        public ControllerRegister Opponent
        {
            get
            {
                return opponent;
            }
        }
    }
}