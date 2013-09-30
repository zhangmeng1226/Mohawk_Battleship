using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using MBC.Core.Events;
using MBC.Shared;

namespace MBC.Core.Rounds
{
    public class RoundReplay : ISerializable
    {
        private List<Event> events;
        private int currentEventIdx;
        private List<IPlayer> playerData;

        public RoundReplay()
        {
            events = new List<Event>();
            currentEventIdx = 0;
        }

        public RoundReplay(Round round)
        {
            events = new List<Event>();
            currentEventIdx = 0;
            round.EventGenerated += AppendEvent;
        }

        protected RoundReplay(SerializationInfo info, StreamingContext context)
        {

        }

        public bool AtEnd
        {
            get
            {
                return currentEventIdx >= events.Count;
            }
        }

        public bool IsPlaying
        {
            get;
            private set;
        }

        public void AppendEvent(Event newEvent)
        {
            events.Add(newEvent);
        }

        public void Play()
        {
            if (IsPlaying)
            {
                return;
            }
            while (IsPlaying && !AtEnd)
            {
                Event currentEvent = events[currentEventIdx++];
                
            }
        }

        protected void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }
    }
}
