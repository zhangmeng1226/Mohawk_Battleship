using System.Runtime.Serialization;
using MBC.Core.Rounds;
using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/> that had lost a <see cref="GameLogic"/>.
    /// </summary>
    public class PlayerLostEvent : PlayerEvent
    {
        public PlayerLostEvent(IDNumber loser)
            : base(loser)
        {
        }

        private PlayerLostEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}