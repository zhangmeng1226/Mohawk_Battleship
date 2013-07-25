using MBC.Core.Rounds;
using System;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Round"/> that has ended.
    /// </summary>
    public class RoundEndEvent : RoundEvent
    {
        /// <summary>
        /// Passes the <paramref name="round"/> to the base constructor and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="round"></param>
        public RoundEndEvent()
        {
        }

        protected internal override void GenerateMessage()
        {
            Message = "This round has ended.";
        }

        internal override void ProcBackward(Round round)
        {
            round.Ended = false;
        }

        internal override void ProcForward(Round round)
        {
            round.Ended = true;
        }
    }
}