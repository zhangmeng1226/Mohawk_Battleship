using MBC.Core.Events;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MBC.Core.Matches
{
    public class EventCollection<EV>
    {
        [XmlIgnore]
        private int currentEventIdx;

        private List<EV> generatedEvents;

        public EventCollection()
        {
            generatedEvents = new List<EV>();
            currentEventIdx = -1;
        }

        [XmlIgnore]
        public EV CurrentEvent
        {
            get
            {
                return generatedEvents[currentEventIdx];
            }
        }

        public void AddEvent(EV ev)
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