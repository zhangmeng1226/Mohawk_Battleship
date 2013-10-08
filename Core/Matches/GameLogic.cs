using System;
using System.Collections.Generic;
using MBC.Core.Events;
using MBC.Core.Matches;
using MBC.Shared;

namespace MBC.Core.Rounds
{
    public abstract class GameLogic
    {
        private ActiveMatch match;

        public GameLogic(IDNumber id, ActiveMatch container)
        {
            ID = id;
            match = container;
        }

        public IDNumber ID
        {
            get;
            private set;
        }

        public abstract bool IsRunning { get; protected set; }

        public ActiveMatch Match
        {
            get;
            private set;
        }

        public abstract void Play();

        public abstract void Stop();

        protected void AddEventAction(Type typeOfEvent, MBCEventHandler eventAction)
        {
            match.AddEventAction(typeOfEvent, eventAction);
        }

        protected void ApplyEvent(Event ev)
        {
            match.ApplyEvent(ev);
        }
    }
}