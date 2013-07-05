using MBC.Core.Rounds;
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
        public RoundEndEvent(Round round)
            : base(round)
        {
            Message = "This round has ended.";
        }

        internal override void ProcForward()
        {
            Round.Ended = true;
        }

        internal override void ProcBackward()
        {
            Round.Ended = false;
        }
    }
}