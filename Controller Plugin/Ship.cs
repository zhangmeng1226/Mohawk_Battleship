namespace MBC.Core
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    /**
     * <summary>A Ship contains information about ship placement and provides various
     * methods to assist with determining various states of the game.
     * 
     * 
     * </summary>
     */
    public sealed class Ship
    {
        private bool isPlaced = false;
        private Point location;
        private ShipOrientation orientation;
        private int length;

        /**
         * <summary>Constructs a Ship with the specified length</summary>
         * <param name="length">The length of this Ship, in number of cells.</param>
         */
        public Ship(int length)
        {
            if (length <= 1)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            this.length = length;
        }

        /**
         * <summary>Gets a bool that indicates whether this ship has been placed.</summary>
         */
        public bool IsPlaced
        {
            get
            {
                return this.isPlaced;
            }
        }

        /**
         * <summary>Gets a Point that indicates the start of the bottom/left of this Ship.</summary>
         */
        public Point Location
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

        /**
         * <summary>Gets the ShipOrientation of this Ship.</summary>
         */
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

        /**
         * <summary>Gets the length of this ship in number of cells on the field.</summary>
         */
        public int Length
        {
            get
            {
                return this.length;
            }
        }

        /**
         * <summary>Used when placing this Ship on the field.</summary>
         * <param name="location">The coordinates of the bottom/left most part of this ship.</param>
         * <param name="orientation">The orientation of this Ship.</param>
         */
        public void Place(Point location, ShipOrientation orientation)
        {
            this.location = location;
            this.orientation = orientation;
            this.isPlaced = true;
        }

        /**
         * <summary>Used to check whether the placement of this ship is valid or not.</summary>
         * <param name="boardSize">The size of the field grid to check boundaries for.</param>
         */
        public bool IsValid(Size boardSize)
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
                if (this.location.Y >= boardSize.Height || this.location.X + this.length > boardSize.Width)
                {
                    return false;
                }
            }
            else
            {
                if (this.location.X >= boardSize.Width || this.location.Y + this.length > boardSize.Height)
                {
                    return false;
                }
            }

            return true;
        }

        /**
         * <summary>Used to check whether this Ship takes up space in the cell located at the specified location.</summary>
         * <param name="location">The cell on the field.</param>
         */
        public bool IsAt(Point location)
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

        /**
         * <summary>Gets a list of Points that represents the cells this Ship occupies.</summary>
         */
        public IEnumerable<Point> GetAllLocations()
        {
            if (this.Orientation == ShipOrientation.Horizontal)
            {
                for (int i = 0; i < this.length; i++)
                {
                    yield return new Point(this.location.X + i, this.location.Y);
                }
            }
            else
            {
                for (int i = 0; i < this.length; i++)
                {
                    yield return new Point(this.location.X, this.location.Y + i);
                }
            }
        }

        /**
         * <summary>Used to check whether this Ship occupies any cell that another Ship occupies.</summary>
         * <param name="otherShip">The other ship to compare with.</param>
         */
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

        /**
         * <summary>Checks if the given list of Points completely occupy the cells this Ship occupies. This
         * is used to determine if shots have sunk this Ship.</summary>
         * <param name="shots">A list of Point objects that represent shots made.</param>
         * <returns>True if the given Points completely occupy the same cells this Ship occupies. False if otherwise.</returns>
         */
        public bool IsSunk(IEnumerable<Point> shots)
        {
            foreach (Point location in this.GetAllLocations())
            {
                if (!shots.Where(s => s.X == location.X && s.Y == location.Y).Any())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
