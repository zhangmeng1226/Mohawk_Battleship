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

        protected internal override void PerformOperation()
        {
            IEnumerable<Coordinates> locations = GetAllLocations();
            foreach (Coordinates coords in locations)
            {
                if (coords.X < 0 || coords.Y < 0 || coords.X > Ship.Owner.Match.FieldSize.X || coords.Y > Ship.Owner.Match.FieldSize.Y)
                {
                    throw new InvalidEventException(this, String.Format("The ship {0} was placed out of bounds.", Ship));
                }
            }
            Ship.Location = Position;
            Ship.Orientation = Orientation;
            Ship.Locations = new HashSet<Coordinates>(locations);
            Ship.RemainingLocations = new HashSet<Coordinates>(Ship.Locations);
            Ship.IsPlaced = true;
            Ship.Active = true;
        }

        private IEnumerable<Coordinates> GetAllLocations()
        {
            if (Orientation == ShipOrientation.Horizontal)
            {
                for (int i = 0; i < Ship.Length; i++)
                {
                    yield return new Coordinates(Position.X + i, Position.Y);
                }
            }
            else
            {
                for (int i = 0; i < Ship.Length; i++)
                {
                    yield return new Coordinates(Position.X, Position.Y + i);
                }
            }
        }
    }
}