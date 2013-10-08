using System.Runtime.Serialization;
using MBC.Core.Rounds;
using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/> that had won a <see cref="GameLogic"/>.
    /// </summary>
    public class PlayerWonEvent : PlayerEvent
    {
        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="register">The <see cref="Register"/> winning a <see cref="GameLogic"/>.</param>
        public PlayerWonEvent(IDNumber player)
            : base(player)
        {
        }

        protected PlayerWonEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}