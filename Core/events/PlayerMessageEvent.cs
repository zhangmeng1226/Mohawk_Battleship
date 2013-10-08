using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using MBC.Shared;

namespace MBC.Core.Events
{
    public class PlayerMessageEvent : Event
    {
        public PlayerMessageEvent(IDNumber playerID, string message)
        {
            PlayerID = playerID;
            Message = message;
        }

        protected PlayerMessageEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public string Message
        {
            get;
            private set;
        }

        public IDNumber PlayerID
        {
            get;
            private set;
        }

        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}