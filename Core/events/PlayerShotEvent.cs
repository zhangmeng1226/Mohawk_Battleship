using System.Runtime.Serialization;
using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Shot"/> that was made by a <see cref="Register"/>.
    /// </summary>
    public class PlayerShotEvent : PlayerEvent
    {
        private Shot shot;

        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor, stores the <paramref name="shot"/>,
        /// and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="register">A <see cref="Register"/> making the <paramref name="shot"/></param>
        /// <param name="shot">The <see cref="Shot"/> made by the <paramref name="register"/>.</param>
        public PlayerShotEvent(IDNumber shooter, Shot shot)
            : base(shooter)
        {
            this.shot = shot;
        }

        protected PlayerShotEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the <see cref="Shot"/> made by the <see cref="PlayerEvent.Register"/>.
        /// </summary>
        public Shot Shot
        {
            get
            {
                return shot;
            }
            private set;
        }

        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}