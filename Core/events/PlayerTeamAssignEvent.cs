using System.Runtime.Serialization;
using MBC.Shared;

namespace MBC.Core.Events
{
    public class PlayerTeamAssignEvent : Event
    {
        public PlayerTeamAssignEvent(IDNumber player, IDNumber teamAssigned)
        {
            PlayerID = player;
            TeamID = teamAssigned;
        }

        protected PlayerTeamAssignEvent(SerializationInfo info, StreamingContext context)
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