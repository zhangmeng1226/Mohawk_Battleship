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

        public PlayerHitShipEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public Shot HitShot
        {
            get;
            private set;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}