using System.Runtime.Serialization;
using MBC.Shared;

namespace MBC.Core.Events
{
    public class PlayerTeamUnassignEvent : Event
    {
        public PlayerTeamUnassignEvent(IDNumber player, IDNumber teamAssigned)
        {
            PlayerID = player;
            TeamID = teamAssigned;
        }

        protected PlayerTeamUnassignEvent(SerializationInfo info, StreamingContext context)
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

        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}