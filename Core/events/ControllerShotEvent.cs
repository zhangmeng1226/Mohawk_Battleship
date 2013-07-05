using MBC.Core.Rounds;
using MBC.Shared;
using System.Text;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Shot"/> that was made by a <see cref="ControllerRegister"/>.
    /// </summary>
    public class ControllerShotEvent : ControllerEvent
    {
        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor, stores the <paramref name="shot"/>,
        /// and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="register">A <see cref="ControllerRegister"/> making the <paramref name="shot"/></param>
        /// <param name="shot">The <see cref="Shot"/> made by the <paramref name="register"/>.</param>
        public ControllerShotEvent(Round rnd, ControllerID register, Shot shot)
            : base(rnd, register)
        {
            Shot = shot;
            StringBuilder msg = new StringBuilder();
            msg.Append(Round.Registers[register]);
            if (shot != null)
            {
                msg.Append(" shot ");
                if (shot.Receiver < Round.MatchInfo.ControllerNames.Count && shot.Receiver >= 0)
                {
                    msg.Append(Round.Registers[Shot.Receiver]);
                }
                else
                {
                    msg.Append("nobody");
                }
                msg.Append(" at ");
                msg.Append(Round.Registers[shot.Receiver]);
                msg.Append(" at ");
                msg.Append(shot.Coordinates);
            }
            else
            {
                msg.Append(" did not make a shot.");
            }
            Message = msg.ToString();
        }

        /// <summary>
        /// Gets the <see cref="Shot"/> made by the <see cref="ControllerEvent.Register"/>.
        /// </summary>
        public Shot Shot
        {
            get;
            private set;
        }

        internal override void ProcForward()
        {
            Round.Registers[RegisterID].Shots.Add(Shot);
            Round.Registers[Shot.Receiver].ShotsAgainst.Add(Shot);
        }

        internal override void ProcBackward()
        {
            Round.Registers[RegisterID].Shots.Remove(Shot);
            Round.Registers[Shot.Receiver].ShotsAgainst.Remove(Shot);
        }
    }
}