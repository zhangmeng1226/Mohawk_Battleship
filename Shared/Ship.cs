using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MBC.Shared
{
    /// <summary>
    /// Provides information about a primary component of a battleship game used by <see cref="Controller"/>s;
    /// the ship. Provides various functions to assist a <see cref="Controller"/> in determining ship
    /// placement, validity, and <see cref="Shot"/> hits made against it.
    /// </summary>
    public class Ship : IEquatable<Ship>
    {
        private bool isPlaced;

        private int length;

        private Coordinates location;

        private ShipOrientation orientation;

        /// <summary>
        /// Copies an existing <see cref="Ship"/>.
        /// </summary>
        /// <param name="shipCopy"></param>
        public Ship(Ship shipCopy)
        {
            IsPlaced = shipCopy.IsPlaced;
            Location = shipCopy.Location;
            Orientation = shipCopy.Orientation;
            Length = shipCopy.Length;
        }

        /// <summary>
        /// Sets the length of the <see cref="Ship"/> to <paramref name="length"/>. The <paramref name="length"/>
        /// must be at least 1.
        /// </summary>
        /// <param name="length">The number of cells the <see cref="Ship"/> occupies.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="length"/> is less
        /// than 1.</exception>
        public Ship(int length)
        {
            if (length < 1)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            Length = length;
        }

        private Ship()
        {
        }

        /// <summary>
        /// Gets a bool that indicates whether or not this <see cref="Ship"/> has been placed.
        /// </summary>
        [XmlIgnore]
        public bool IsPlaced
        {
            get
            {
                return isPlaced;
            }
            private set
            {
                isPlaced = value;
            }
        }

        /// <summary>
        /// Gets the length by number of cells occupied.
        /// </summary>
        [XmlIgnore]
        public int Length
        {
            get
            {
                return length;
            }
            set
            {
                length = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="Coordinates"/> of placement.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this <see cref="Ship"/> has not been placed.</exception>
        public Coordinates Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="ShipOrientation"/>.
        /// </summary>
        public ShipOrientation Orientation
        {
            get
            {
                return orientation;
            }
            set
            {
                orientation = value;
            }
        }

        /// <summary>
        /// Checks if another <see cref="Ship"/> occupies any cells that this <see cref="Ship"/> occupies.
        /// </summary>
        /// <param name="otherShip">The <see cref="Ship"/> to check for confliction with.</param>
        /// <returns>A value indicating the state of conflicting with the <paramref name="otherShip"/>.</returns>
        public bool ConflictsWith(Ship otherShip)
        {
            foreach (var otherShipLocation in otherShip.GetAllLocations())
            {
                if (this.IsAt(otherShipLocation))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not this <see cref="Ship"/> is equal to another <paramref name="ship"/> in
        /// both of the values in their fields.
        /// </summary>
        /// <param name="ship">The <see cref="Ship"/> to compare to.</param>
        /// <returns>A value indicating whether or not a <see cref="Ship"/> is equivalent.</returns>
        public bool Equals(Ship ship)
        {
            if (ship == null)
            {
                return false;
            }
            return IsPlaced == ship.IsPlaced &&
                Location == ship.Location &&
                Orientation == ship.Orientation &&
                Length == ship.Length;
        }

        /// <summary>
        /// Determines whether or not this <see cref="Ship"/> is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        public override bool Equals(object obj)
        {
            return Equals(obj as Ship);
        }

        /// <summary>
        /// Gets a list of <see cref="Coordinates"/> that contains all of the locations occupied.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="Coordinates"/>.</returns>
        public IEnumerable<Coordinates> GetAllLocations()
        {
            if (Orientation == ShipOrientation.Horizontal)
            {
                for (int i = 0; i < Length; i++)
                {
                    yield return new Coordinates(Location.X + i, Location.Y);
                }
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    yield return new Coordinates(Location.X, Location.Y + i);
                }
            }
        }

        /// <summary>
        /// Generates a hash code of this <see cref="Ship"/> based on its fields.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            int hash = 23;
            hash = hash * 37 + IsPlaced.GetHashCode();
            hash = hash * 37 + Location.GetHashCode();
            hash = hash * 37 + Orientation.GetHashCode();
            hash = hash * 37 + Length.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Indicates whether or not a <paramref name="location"/> is an occupied cell.
        /// </summary>
        /// <param name="location">The <see cref="Coordinates"/> of the location to check.</param>
        /// <returns>A value indicating the presence of this <see cref="Ship"/> at the <paramref name="location"/>.</returns>
        public bool IsAt(Coordinates location)
        {
            if (!IsPlaced)
            {
                return false;
            }
            if (Orientation == ShipOrientation.Horizontal)
            {
                return (Location.Y == location.Y) && (Location.X <= location.X) && (Location.X + Length > location.X);
            }
            else
            {
                return (Location.X == location.X) && (Location.Y <= location.Y) && (Location.Y + Length > location.Y);
            }
        }

        /// <summary>Checks if the given list of Coordinates completely occupy the cells this Ship occupies. This
        /// is used to determine if shots have sunk this Ship.</summary>
        /// <param name="shots">A list of Coordinates that represent shots made.</param>
        /// <returns>True if the Coordinates in the given ShotList completely occupy the same cells this Ship occupies. False if otherwise.</returns>
        public bool IsSunk(ShotList shots)
        {
            foreach (var location in GetAllLocations())
            {
                if (!shots.Contains(location))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Indicated whether or not placement of this <see cref="Ship"/> is valid.
        /// </summary>
        /// <param name="boardSize">The maximum size of a field in the <see cref="Coordinates.X"/>
        /// and <see cref="Coordinates.Y"/> components.</param>
        /// <returns>A value indicating valid <see cref="Ship"/> placement.</returns>
        public bool IsValid(Coordinates boardSize)
        {
            if (!IsPlaced)
            {
                return false;
            }

            if (Location < new Coordinates(0, 0))
            {
                return false;
            }

            if (Orientation == ShipOrientation.Horizontal)
            {
                if (new Coordinates(Location.X + Length, Location.Y) >= boardSize)
                {
                    return false;
                }
            }
            else
            {
                if (new Coordinates(Location.X, Location.Y) >= boardSize)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Places this <see cref="Ship"/> with the <paramref name="location"/> and <paramref name="orientation"/>.
        /// Sets the state of being placed to true.
        /// </summary>
        /// <param name="location">The <see cref="Coordinates"/> of the bottom-most/left-most part of
        /// the <see cref="Ship"/>.</param>
        /// <param name="orientation">The <see cref="ShipOrientation"/> to set.</param>
        public void Place(Coordinates location, ShipOrientation orientation)
        {
            Location = location;
            Orientation = orientation;
            IsPlaced = true;
        }

        /// <summary>
        /// Provides a string representation.
        /// </summary>
        /// <returns>A string representation of this <see cref="Ship"/>.</returns>
        public override string ToString()
        {
            return Location + " L" + Length + " " + Enum.GetName(typeof(ShipOrientation), Orientation);
        }
    }
}