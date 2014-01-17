using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Do not use.
    /// </summary>
    [Obsolete("Unnecessary event.")]
    public class PlayerTeamUnassignEvent : Event
    {
        public PlayerTeamUnassignEvent(IDNumber player, IDNumber teamAssigned)
        {
            PlayerID = player;
            TeamID = teamAssigned;
        }

        public PlayerTeamUnassignEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IDNumber PlayerID
        {
            get;
            private set;
        }

        public IDNumber TeamID
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