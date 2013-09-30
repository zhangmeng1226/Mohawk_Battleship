using MBC.Core.Rounds;
using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// A <see cref="RoundEvent"/> that provides information on a turn change.
    /// </summary>
    public class RoundPlayerActionEvent : RoundEvent
    {
        /// <summary>
        /// Sets the previous and next <see cref="IDNumber"/>s in the switch.
        /// </summary>
        /// <param name="last">The previous <see cref="IDNumber"/> that had the turn.</param>
        /// <param name="next">The next <see cref="IDNumber"/> to take the turn.</param>
        public RoundPlayerActionEvent(IDNumber roundID, IDNumber player)
            : base(roundID)
        {
            PlayerID = player;
        }

        private RoundPlayerActionEvent(SerializationInfo info, StreamingContext context)
        {

        }

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        public virtual Type EventType
        {
            get
            {
                return Type.RoundPlayerAction;
            }
        }

        public IDNumber PlayerID
        {
            get;
            private set;
        }
    }
}