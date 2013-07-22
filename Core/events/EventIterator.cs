using MBC.Core.Events;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MBC.Core.Matches
{
    public class EventIterator
    {
        [XmlIgnore]
        private int currentEventIdx;

        private List<Event> generatedEvents;

        public EventIterator()
        {
            generatedEvents = new List<Event>();
            currentEventIdx = -1;
        }

        [XmlIgnore]
        public Event CurrentEvent
        {
            get
            {
                return generatedEvents[currentEventIdx];
            }
        }

        public void AddEvent(Event ev)
        {
            generatedEvents.Add(ev);
            currentEventIdx = generatedEvents.Count - 1;
        }

        public bool StepBackward()
        {
            if (--currentEventIdx < 0)
            {
                currentEventIdx = 0;
                return true;
            }
            return false;
        }

        public bool StepForward()
        {
            if (++currentEventIdx > generatedEvents.Count - 1)
            {
                currentEventIdx = generatedEvents.Count - 1;
                return true;
            }
            return false;
        }
    }
}