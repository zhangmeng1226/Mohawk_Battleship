﻿using MBC.Shared.Entities;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Defines a method that retrieves and handles an <see cref="Event"/>.
    /// </summary>
    /// <param name="ev">The generated <see cref="Event"/></param>
    public delegate void MBCEventHandler(Event ev);

    /// <summary>
    /// The base class for any event created in the MBC core framework. Provides a message string that
    /// describes the generated event.
    /// </summary>
    /// <seealso cref="MatchEvent"/>
    /// <seealso cref="RoundEvent"/>
    /// <seealso cref="PlayerEvent"/>
    public abstract class Event
    {
        protected Event(Entity creatingEntity)
        {
            Entity = creatingEntity;
            Millis = (int)creatingEntity.GameTimer.ElapsedMilliseconds;
        }

        public int Millis
        {
            get;
            private set;
        }

        protected internal Entity Entity
        {
            get;
            private set;
        }

        protected internal virtual void PerformOperation()
        {
        }
    }
}