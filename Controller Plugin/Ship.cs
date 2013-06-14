namespace MBC.Core
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    /// <summary>A Ship contains information about the placement of a ship, and provides various
    /// methods to assist with determining various states of the game.
    /// </summary>
    public class Ship : IEquatable<Ship>
    {
        private bool isPlaced = false;
        private Coordinates location;
        private ShipOrientation orientation;
        private int length;

        /// <summary>
        /// Copies a Ship object.
        /// </summary>
        /// <param name="shipCopy"></param>
        public Ship(Ship shipCopy)
        {
            isPlaced = shipCopy.isPlaced;
            location = shipCopy.location;
            orientation = shipCopy.orientation;
            length = shipCopy.length;
        }

        /// <summary>Constructs a Ship with the specified length</summary>
        /// <param name="length">The length of this Ship, in number of cells.</param>
        public Ship(int length)
        {
            if (length <= 1)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            this.length = length;
        }

        /// <summary>Gets a bool that indicates whether this ship has been placed.</summary>
        public bool IsPlaced
        {
            get
            {
                return this.isPlaced;
            }
        }

        /// <summary>Gets the Coordinates that indicate the start of the bottom/left of this Ship.</summary>
        public Coordinates Location
        {
            get
            {
                if (!this.isPlaced)
                {
                    throw new InvalidOperationException();
                }

                return this.location;
            }
        }

        /// <summary>Gets the ShipOrientation of this Ship.</summary>
        public ShipOrientation Orientation
        {
            get
            {
                if (!this.isPlaced)
                {
                    throw new InvalidOperationException();
                }

                return this.orientation;
            }
        }

        /// <summary>Gets the length of this ship in number of cells on the field.</summary>
        public int Length
        {
            get
            {
                return this.length;
            }
        }

        /// <summary>Used when placing this Ship on the field.</summary>
        /// <param name="location">The Coordinates of the bottom/left most part of this ship.</param>
        /// <param name="orientation">The orientation of this Ship.</param>
        public void Place(Coordinates location, ShipOrientation orientation)
        {
            this.location = location;
            this.orientation = orientation;
            this.isPlaced = true;
        }

        /// <summary>Used to check whether the placement of this ship is valid or not.</summary>
        /// <param name="boardSize">The size of the field grid to check boundaries for.</param>
        public bool IsValid(Coordinates boardSize)
        {
            if (!this.isPlaced)
            {
                return false;
            }

            if (this.location.X < 0 || this.location.Y < 0)
            {
                return false;
            }

            if (this.orientation == ShipOrientation.Horizontal)
            {
                if (this.location.Y >= boardSize.Y || this.location.X + this.length > boardSize.X)
                {
                    return false;
                }
            }
            else
            {
                if (this.location.X >= boardSize.X || this.location.Y + this.length > boardSize.Y)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>Used to check if this Ship takes up space in the cell located at the specified location.</summary>
        /// <param name="location">The cell on the field.</param>
        public bool IsAt(Coordinates location)
        {
            if (this.Orientation == ShipOrientation.Horizontal)
            {
                return (this.location.Y == location.Y) && (this.location.X <= location.X) && (this.location.X + this.length > location.X);
            }
            else
            {
                return (this.location.X == location.X) && (this.location.Y <= location.Y) && (this.location.Y + this.length > location.Y);
            }
        }

        /// <summary>Gets a list of Coordinates that represents the cells this Ship occupies.</summary>
        public IEnumerable<Coordinates> GetAllLocations()
        {
            if (this.Orientation == ShipOrientation.Horizontal)
            {
                for (int i = 0; i < this.length; i++)
                {
                    yield return new Coordinates(this.location.X + i, this.location.Y);
                }
            }
            else
            {
                for (int i = 0; i < this.length; i++)
                {
                    yield return new Coordinates(this.location.X, this.location.Y + i);
                }
            }
        }

        /// <summary>Used to check whether this Ship occupies any cell that another Ship occupies.</summary>
        /// <param name="otherShip">The other ship to compare with.</param>
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
        /// Determines whether or not this Ship is equal to another Ship in terms of the values of the fields.
        /// </summary>
        /// <param name="ship">The Ship to compare to.</param>
        /// <returns>true if the fields in this Ship are equal to the values in the other Ship.</returns>
        public bool Equals(Ship ship)
        {
            if (ship == null)
            {
                return false;
            }
            return isPlaced == ship.isPlaced &&
                location == ship.location &&
                orientation == ship.orientation &&
                length == ship.length;
        }

        /// <summary>
        /// Provides a string representation of the Ship object.
        /// </summary>
        /// <returns>The string representing this Ship.</returns>
        public override string ToString()
        {
            return location + " L" + length + " " + Enum.GetName(typeof(ShipOrientation), orientation);
        }

        /// <summary>
        /// Generates a hash code of this Ship based on its fields.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            int hash = 23;
            hash = hash * 37 + isPlaced.GetHashCode();
            hash = hash * 37 + location.GetHashCode();
            hash = hash * 37 + orientation.GetHashCode();
            hash = hash * 37 + length.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Determines whether or not this Ship is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        public override bool Equals(object obj)
        {
            return Equals(obj as Ship);
        }
    }
}
