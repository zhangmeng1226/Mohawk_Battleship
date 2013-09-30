using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using MBC.Core.Players;
using MBC.Shared;

namespace MBC.Core.Events
{
    public class RoundStateSaveEvent : Event
    {
        public RoundStateSaveEvent(List<IPlayer> players)
        {
            SavedPlayers = new List<IPlayer>();
            foreach (var player in players)
            {
                SavedPlayers.Add(new Player(player));
            }
        }

        public RoundStateSaveEvent(SerializationInfo info, StreamingContext context)
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        public override Type EventType
        {
            get
            {
                return Type.RoundStateSave;
            }
        }

        public List<IPlayer> SavedPlayers
        {
            get;
            private set;
        }
    }
}
