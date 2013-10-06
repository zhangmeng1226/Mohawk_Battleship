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

        public PlayerTeamUnassignEvent(SerializationInfo info, StreamingContext context)
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}