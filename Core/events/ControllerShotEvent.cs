using MBC.Shared;
using System.Text;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Shot"/> that was made by a <see cref="ControllerRegister"/>.
    /// </summary>
    public class ControllerShotEvent : ControllerEvent
    {
        private Shot shot;

        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor, stores the <paramref name="shot"/>,
        /// and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="register">A <see cref="ControllerRegister"/> making the <paramref name="shot"/></param>
        /// <param name="shot">The <see cref="Shot"/> made by the <paramref name="register"/>.</param>
        public ControllerShotEvent(ControllerRegister register, Shot shot)
            : base(register)
        {
            this.shot = shot;
            StringBuilder msg = new StringBuilder();
            msg.Append(register);
            if (shot != null)
            {
                msg.Append(" shot ");
                if (shot.Receiver < register.Match.Players.Count && shot.Receiver >= 0)
                {
                    msg.Append(register.Match.Players[shot.Receiver]);
                }
                else
                {
                    msg.Append("nobody");
                }
                msg.Append(" at ");
                msg.Append(shot.Coordinates);
            }
            else
            {
                msg.Append(" did not make a shot.");
            }
            message = msg.ToString();
        }

        /// <summary>
        /// Gets the <see cref="Shot"/> made by the <see cref="ControllerEvent.Register"/>.
        /// </summary>
        public Shot Shot
        {
            get
            {
                return shot;
            }
        }
    }
}