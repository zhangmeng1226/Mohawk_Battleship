using System.Runtime.Serialization;
using MBC.Shared;

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
    }
}