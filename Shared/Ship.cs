using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MBC.Shared
{
    /// <summary>
    /// Provides information about a primary component of a battleship game used by <see cref="Controller"/>s;
    /// the ship. Provides various functions to assist a <see cref="Controller"/> in determining ship
    /// placement, validity, and <see cref="Shot"/> hits made against it.
    /// </summary>
    [Serializable]
    public class Ship : Entity, IEquatable<Ship>
    {
        /// <summary>
        /// Copies an existing <see cref="Ship"/>.
        /// </summary>
        /// <param name="shipCopy"></param>
        public Ship(Ship shipCopy)
        {
            ID = shipCopy.ID;
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
            : this(length, -1)
        {
        }

        /// <summary>
        /// Sets the length of the <see cref="Ship"/> to <paramref name="length"/>. The <paramref name="length"/>
        /// must be at least 1.
        /// </summary>
        /// <param name="length">The number of cells the <see cref="Ship"/> occupies.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="length"/> is less
        /// than 1.</exception>
        public Ship(int length, IDNumber identifier)
        {
            ID = identifier;
            if (length < 1 || length > 31)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            Length = length;
        }

        /// <summary>
        /// Gets the ID number that uniquely identifies the ship within a collection of ships.
        /// </summary>
        public IDNumber ID
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets a bool that indicates whether or not this <see cref="Ship"/> has been placed.
        /// </summary>
        public bool IsPlaced
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets the length by number of cells occupied. When set, sets the array of
        /// shots made against this ship to the value.
        /// </summary>
        public int Length
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets the <see cref="Coordinates"/> of placement.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this <see cref="Ship"/> has not been placed.</exception>
        public Coordinates Location
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets the <see cref="ShipOrientation"/>.
        /// </summary>
        public ShipOrientation Orientation
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets the player that owns the ship.
        /// </summary>
        public Player Owner
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets an integer that represents a bit-set of ship hits made against the ship.
        /// </summary>
        protected internal int Shots
        {
            get;
            set;
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

        /// <summary>
        /// Checks whether the hit flag has been set at the specific coordinates.
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public bool IsHitAt(Coordinates coords)
        {
            if (IsAt(coords))
            {
                switch (Orientation)
                {
                    case ShipOrientation.Horizontal:
                        return IsHitAt(Location.X - coords.X);

                    case ShipOrientation.Vertical:
                        return IsHitAt(Location.Y - coords.Y);
                }
            }
            return false;
        }

        /// <summary>
        /// Checks whether the hit flag has been set at the specific ship location index.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool IsHitAt(int idx)
        {
            var idxPow = 1 << idx;
            return (Shots & idxPow) == idxPow;
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
        /// Checks if the shot hits made against this ship via SetShotHit are all true, which
        /// indicates a sunken ship.
        /// </summary>
        /// <returns></returns>
        public bool IsSunk()
        {
            int sunkVal = 0;
            for (int i = 1; i <= Length; i++)
            {
                sunkVal |= 1 << i;
            }
            return sunkVal == Shots;
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
            InvokeEvent(new ShipMovedEvent(this, location, orientation));
        }

        public void Sink()
        {
            InvokeEvent(new ShipDestroyedEvent(this));
        }

        /// <summary>
        /// Provides a string representation.
        /// </summary>
        /// <returns>A string representation of this <see cref="Ship"/>.</returns>
        public override string ToString()
        {
            return Location + " L" + Length + " " + Enum.GetName(typeof(ShipOrientation), Orientation);
        }

        /// <summary>
        /// Hits the ship at the specified index.
        /// </summary>
        /// <param name="idx"></param>
        internal void Hit(int idx)
        {
            InvokeEvent(new ShipHitEvent(this, idx));
        }

        /// <summary>
        /// Attempts to hit the ship at the given coordinates.
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        internal bool Hit(Coordinates coords)
        {
            if (IsAt(coords))
            {
                switch (Orientation)
                {
                    case ShipOrientation.Horizontal:
                        Hit(Location.X - coords.X);
                        break;

                    case ShipOrientation.Vertical:
                        Hit(Location.Y - coords.Y);
                        break;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Resets the ship back to its initialized state.
        /// </summary>
        internal void Reset()
        {
            Location = default(Coordinates);
            Orientation = default(ShipOrientation);
            IsPlaced = false;
            Shots = 0;
        }
    }
}