using System.Runtime.Serialization;
using MBC.Shared;

namespace MBC.Core.Events
{
    public class PlayerHitShipEvent : PlayerEvent
    {
        public PlayerHitShipEvent(IDNumber sender, Shot shot)
            : base(sender)
        {
            HitShot = shot;
        }

        private PlayerHitShipEvent(SerializationInfo info, StreamingContext context)
        {
        }

        public Shot HitShot
        {
            get;
            private set;
        }

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}