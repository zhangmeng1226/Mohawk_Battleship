using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;

namespace MBC.Core.Events
{
    public delegate void MBCSaveStateRequest(StateSave ess);

    public sealed class EventDriver : ISerializable
    {
        private int currentEventIdx;
        private List<Event> events;
        private AutoResetEvent signal;
        private List<StateSave> stateSaves;

        public EventDriver()
        {
            events = new List<Event>();
            stateSaves = new List<StateSave>();
            currentEventIdx = 0;
            signal = new AutoResetEvent(false);
        }

        public EventDriver(SerializationInfo info, StreamingContext context)
            : this()
        {
            //Deserialize
        }

        public event MBCEventHandler EventApplied;

        protected internal event MBCSaveStateRequest StateLoadRequest;

        protected internal event MBCSaveStateRequest StateSaveRequest;

        public bool AtEnd
        {
            get
            {
                return currentEventIdx == events.Count - 1;
            }
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        public void AddEvent(Event ev)
        {
            events.Add(ev);
        }

        public void ApplyEvent(Event ev)
        {
            events.Add(ev);
            if (currentEventIdx == events.Count - 1)
            {
                RaiseAppliedEvent(ev);
                currentEventIdx++;
            }
            else
            {
                Play();
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //Serialize
        }

        public void Play(float timeScale)
        {
            if (IsRunning || events.Count == 0 || currentEventIdx == events.Count)
            {
                return;
            }
            IsRunning = true;
            signal.Reset();
            while (IsRunning && currentEventIdx != events.Count - 1)
            {
                var applyEvent = events[currentEventIdx++];
                var sleepTime = DateTime.Now.Millisecond;

                RaiseAppliedEvent(applyEvent);
                sleepTime = (events[currentEventIdx].Millis - applyEvent.Millis) - (DateTime.Now.Millisecond - sleepTime);

                signal.WaitOne((int)(sleepTime * timeScale));
            }
            if (IsRunning)
            {
                RaiseAppliedEvent(events[currentEventIdx++]);
            }
        }

        public void Play()
        {
            if (IsRunning || events.Count == 0 || currentEventIdx == events.Count)
            {
                return;
            }
            IsRunning = true;
            while (IsRunning && currentEventIdx != events.Count)
            {
                RaiseAppliedEvent(events[currentEventIdx++]);
            }
        }

        public void Stop()
        {
            IsRunning = false;
            signal.Set();
        }

        private void RaiseAppliedEvent(Event ev)
        {
            if (EventApplied != null)
            {
                EventApplied(ev);
            }
        }
    }
}