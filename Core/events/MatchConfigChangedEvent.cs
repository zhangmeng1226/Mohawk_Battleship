using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using MBC.Shared;

namespace MBC.Core.Events
{
    public class MatchConfigChangedEvent : Event
    {
        public MatchConfigChangedEvent(MatchConfig config)
        {
            Config = new MatchConfig(config);
            Config.Registers = null;
            Config.Teams = null;
        }

        public MatchConfigChangedEvent(SerializationInfo info, StreamingContext context)
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        public override Event.Type EventType
        {
            get
            {
                return Type.MatchConfigChanged;
            }
        }

        public MatchConfig Config
        {
            get;
            private set;
        }
    }
}
