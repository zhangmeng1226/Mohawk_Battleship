using MBC.Core.Events;
using MBC.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Matches
{
    public class EventIterator
    {
        private List<Event> generatedEvents;
        private int currentEventIdx;

        public EventIterator()
        {
            generatedEvents = new List<Event>();
            currentEventIdx = -1;
        }

        public void AddEvent(Event ev)
        {
            generatedEvents.Add(ev);
            currentEventIdx = generatedEvents.Count - 1;
        }

        public Event CurrentEvent
        {
            get
            {
                return generatedEvents[currentEventIdx];
            }
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

        public bool StepBackward()
        {
            if (--currentEventIdx < 0)
            {
                currentEventIdx = 0;
                return true;
            }
            return false;
        }
    }
}
