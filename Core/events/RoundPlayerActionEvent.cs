using MBC.Core.Rounds;
using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{

    public class RoundPlayerActionEvent : RoundEvent
    {
        public RoundPlayerActionEvent(IDNumber roundID)
            : base(roundID)
        {
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
    }
}