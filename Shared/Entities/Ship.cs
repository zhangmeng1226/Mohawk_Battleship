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
    public class Ship : Entity
    {
        /// <summary>
        /// Copies an existing <see cref="Ship"/>.
        /// </summary>
        /// <param name="shipCopy"></param>
        public Ship(Ship shipCopy)
            : base(shipCopy.ID)
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
        public Ship(IDNumber id, int length)
            : base(id)
        {
            if (length < 1 || length > 31)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            Length = length;
        }

        public bool Active
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

        public HashSet<Coordinates> Locations
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

        public HashSet<Coordinates> RemainingLocations
        {
            get;
            protected internal set;
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
        /// Gets a list of <see cref="Coordinates"/> that contains all of the locations occupied.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="Coordinates"/>.</returns>
        [Obsolete("Use the property Locations instead.")]
        public IEnumerable<Coordinates> GetAllLocations()
        {
            return Locations;
        }

        /// <summary>
        /// Hits the ship at the coordinates.
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public void Hit(Coordinates coords)
        {
            InvokeEvent(new ShipHitEvent(this, coords));
        }

        /// <summary>
        /// Indicates whether or not a <paramref name="location"/> is an occupied cell.
        /// </summary>
        /// <param name="location">The <see cref="Coordinates"/> of the location to check.</param>
        /// <returns>A value indicating the presence of this <see cref="Ship"/> at the <paramref name="location"/>.</returns>
        public bool IsAt(Coordinates location)
        {
            return IsPlaced && Locations.Contains(location);
        }

        /// <summary>
        /// Checks whether the hit flag has been set at the specific ship location index.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool IsHitAt(Coordinates location)
        {
            return !RemainingLocations.Contains(location);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool IsSunk()
        {
            return RemainingLocations.Count == 0;
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

        /// <summary>
        /// Resets the ship back to its initialized state.
        /// </summary>
        public virtual void Reset()
        {
            InvokeEvent(new ShipResetEvent(this));
        }

        /// <summary>
        /// Sinks the ship
        /// </summary>
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
            return String.Format("[{0}] L{1}", ID, Length);
        }
    }
}