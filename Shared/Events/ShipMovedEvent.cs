using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/>'s <see cref="ShipList"/> that had
    /// been requested to be modified.
    /// </summary>
    [Serializable]
    public class ShipMovedEvent : ShipEvent
    {
        public ShipMovedEvent(Ship ship, Coordinates position, ShipOrientation orientation)
            : base(ship)
        {
            Position = position;
            Orientation = orientation;
        }

        public ShipOrientation Orientation
        {
            get;
            protected set;
        }

        public Coordinates Position
        {
            get;
            protected set;
        }
    }
}