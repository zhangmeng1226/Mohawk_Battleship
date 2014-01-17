using MBC.Core.Events;
using MBC.Core.Matches;
using MBC.Shared;
using System;
using System.Collections.Generic;

namespace MBC.Core.Rounds
{
    [Obsolete]
    public abstract class GameLogic
    {
        public GameLogic(IDNumber id, ActiveMatch container)
        {
            ID = id;
            Match = container;
        }

        public abstract bool Ended { get; }

        public IDNumber ID
        {
            get;
            private set;
        }

        public ActiveMatch Match
        {
            get;
            private set;
        }

        public abstract void Play();

        public abstract void Stop();

        protected void AddEventAction(Type typeOfEvent, MBCEventHandler eventAction)
        {
            Match.AddEventAction(typeOfEvent, eventAction);
        }

        protected void ApplyEvent(Event ev)
        {
            Match.ApplyEvent(ev);
        }
    }
}