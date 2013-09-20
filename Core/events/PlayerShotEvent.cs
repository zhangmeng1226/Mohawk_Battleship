using MBC.Core.Rounds;
using MBC.Shared;
using System;
using System.Text;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Shot"/> that was made by a <see cref="Register"/>.
    /// </summary>
    public class PlayerShotEvent : PlayerEvent
    {
        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor, stores the <paramref name="shot"/>,
        /// and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="register">A <see cref="Register"/> making the <paramref name="shot"/></param>
        /// <param name="shot">The <see cref="Shot"/> made by the <paramref name="register"/>.</param>
        public PlayerShotEvent(Player shooter, Shot shot)
            : base(shooter)
        {
            Shot = shot;
        }


        protected internal override void GenerateMessage()
        {
            StringBuilder msg = new StringBuilder();
            msg.Append(Player);
            if (Shot != null)
            {
                msg.Append(" shot ");
                msg.Append(Shot.Receiver);
                msg.Append(" at ");
                msg.Append(Shot.Coordinates);
            }
            else
            {
                msg.Append(" did not make a shot.");
            }
            Message = msg.ToString();
        }

        /// <summary>
        /// Gets the <see cref="Shot"/> made by the <see cref="PlayerEvent.Register"/>.
        /// </summary>
        public Shot Shot
        {
            get;
            private set;
        }

        internal override void ProcBackward(Round round)
        {
            round.Registers[PlayerID].Shots.Remove(Shot);
            round.Registers[Shot.Receiver].ShotsAgainst.Remove(Shot);
        }

        internal override void ProcForward(Round round)
        {
            round.Registers[PlayerID].Shots.Add(Shot);
            round.Registers[Shot.Receiver].ShotsAgainst.Add(Shot);
        }
    }
}