using MBC.Core.Rounds;
using MBC.Shared;
using System;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Shot"/> made by a <see cref="Register"/> against
    /// an opposing <see cref="Register"/>.
    /// </summary>
    public class PlayerHitShipEvent : PlayerEvent
    {
        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor, stores the parameters, and
        /// generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="register">The <see cref="Register"/> creating the <see cref="Shot"/></param>
        /// <param name="opposer">The receiving <see cref="Register"/> of the <see cref="Shot"/>.</param>
        /// <param name="shot">The <see cref="Shot"/> created.</param>
        public PlayerHitShipEvent(Player sender, Shot shot)
            : base(sender)
        {
            HitShot = shot;
        }

        protected internal override void GenerateMessage()
        {
            return Player + " hit " + HitShot.Receiver + "'s ship at " + HitShot.Coordinates;
        }

        /// <summary>
        /// Gets the <see cref="Shot"/> that hit the <see cref="PlayerHitShipEvent.Opponent"/>.
        /// </summary>
        public Shot HitShot
        {
            get;
            private set;
        }
    }
}