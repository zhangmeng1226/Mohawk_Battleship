using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Shot"/> made by a <see cref="ControllerRegister"/> against
    /// an opposing <see cref="ControllerRegister"/>.
    /// </summary>
    public class ControllerHitShipEvent : ControllerEvent
    {
        private Shot coords;
        private ControllerRegister opponent;

        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor, stores the parameters, and
        /// generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="register">The <see cref="ControllerRegister"/> creating the <see cref="Shot"/></param>
        /// <param name="opposer">The receiving <see cref="ControllerRegister"/> of the <see cref="Shot"/>.</param>
        /// <param name="shot">The <see cref="Shot"/> created.</param>
        public ControllerHitShipEvent(ControllerRegister register, ControllerRegister opposer, Shot shot)
            : base(register)
        {
            this.coords = shot;
            this.opponent = opposer;

            message = register + " hit a " + opposer + " ship at " + shot.Coordinates;
        }

        /// <summary>
        /// Gets the <see cref="Shot"/> that hit the <see cref="ControllerHitShipEvent.Opponent"/>.
        /// </summary>
        public Shot HitShot
        {
            get
            {
                return coords;
            }
        }

        /// <summary>
        /// Gets the receiving <see cref="ControllerRegister"/> of the <see cref="ControllerHitShipEvent.HitShot"/>;
        /// </summary>
        public ControllerRegister Opponent
        {
            get
            {
                return opponent;
            }
        }
    }
}