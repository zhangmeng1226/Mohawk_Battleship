using MBC.Core.Rounds;
using MBC.Shared;
using System;
using System.Runtime.Serialization;

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

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        public Shot HitShot
        {
            get;
            private set;
        }
    }
}