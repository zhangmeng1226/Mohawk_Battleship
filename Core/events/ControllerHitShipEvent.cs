using MBC.Core.Rounds;
using MBC.Shared;
using System;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Shot"/> made by a <see cref="ControllerRegister"/> against
    /// an opposing <see cref="ControllerRegister"/>.
    /// </summary>
    public class ControllerHitShipEvent : ControllerEvent
    {
        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor, stores the parameters, and
        /// generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="register">The <see cref="ControllerRegister"/> creating the <see cref="Shot"/></param>
        /// <param name="opposer">The receiving <see cref="ControllerRegister"/> of the <see cref="Shot"/>.</param>
        /// <param name="shot">The <see cref="Shot"/> created.</param>
        public ControllerHitShipEvent(ControllerID sender, Shot shot)
            : base(sender)
        {
            HitShot = shot;
        }

        protected internal override void GenerateMessage()
        {
            Message = RegisterID + " hit a " + HitShot.Receiver + " ship at " + HitShot.Coordinates;
        }

        /// <summary>
        /// Gets the <see cref="Shot"/> that hit the <see cref="ControllerHitShipEvent.Opponent"/>.
        /// </summary>
        public Shot HitShot
        {
            get;
            private set;
        }
    }
}