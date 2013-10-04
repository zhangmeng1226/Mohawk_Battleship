﻿using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// The base class for a series of <see cref="Event"/>s that provide information on the actions
    /// made in rounds from controllers. Provides the <see cref="IDNumber"/> of the <see cref="Controller"/>
    /// that caused the event to be created.
    /// </summary>
    public abstract class PlayerEvent : Event
    {
        public PlayerEvent(IDNumber plrID)
        {
            Player = plrID;
        }

        protected PlayerEvent()
        {
        }

        public IDNumber Player
        {
            get;
            private set;
        }
    }
}