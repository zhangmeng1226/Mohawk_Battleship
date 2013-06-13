using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core
{
    /// <summary>
    /// Coordinates are used to represent X and Y Coordinates. Coordinates can be operated on with the following
    /// operators:
    /// <list type="bullet">
    /// <item>+</item>
    /// <item>-</item>
    /// <item>==</item>
    /// <item>!=</item>
    /// <item>></item>
    /// <item><</item>
    /// <item>>=</item>
    /// <item><=</item>
    /// </list>
    /// </summary>
    public struct Coordinates : IEquatable<Coordinates>
    {
        private int x;
        private int y;

        /// <summary>
        /// Sets the X and Y component of these Coordinates to the given values.
        /// </summary>
        /// <param name="x">The X value.</param>
        /// <param name="y">The Y value.</param>
        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Adds the values of two Coordinates.
        /// </summary>
        /// <param name="coord1">The Coordinates to add to.</param>
        /// <param name="coord2">The Coordinates to add.</param>
        /// <returns>The resulting Coordinates.</returns>
        public static Coordinates operator +(Coordinates coord1, Coordinates coord2)
        {
            return new Coordinates(coord1.x + coord2.x, coord1.y + coord2.y);
        }

        /// <summary>
        /// Subtracts the value of a set of Coordinates from another set of Coordinates.
        /// </summary>
        /// <param name="coord1">The Coordinates to subtract values from.</param>
        /// <param name="coord2">The Coordinates to subtract.</param>
        /// <returns>The resulting Coordinates.</returns>
        public static Coordinates operator -(Coordinates coord1, Coordinates coord2)
        {
            return new Coordinates(coord1.x - coord2.x, coord1.y - coord2.y);
        }

        /// <summary>
        /// Compares two Coordinates for equality.
        /// </summary>
        /// <param name="coord1">Coordinates to compare.</param>
        /// <param name="coord2">Coordinates to compare to the other set.</param>
        /// <returns>true if the given Coordinates are equal, false otherwise.</returns>
        public static bool operator ==(Coordinates coord1, Coordinates coord2)
        {
            return coord1.x == coord2.x && coord1.y == coord2.y;
        }

        /// <summary>
        /// Compares two Coordinates for inequality.
        /// </summary>
        /// <param name="coord1">Coordinates to compare.</param>
        /// <param name="coord2">Coordinates to compare to the other set.</param>
        /// <returns>true if the given Coordinates are inequal, false otherwise.</returns>
        public static bool operator !=(Coordinates coord1, Coordinates coord2)
        {
            return !(coord1 == coord2);
        }

        /// <summary>
        /// Compares two Coordinates, determining if a set of Coordinates is greater than the other
        /// in both components.
        /// </summary>
        /// <param name="coord1">The Coordinates to compare.</param>
        /// <param name="coord2">The Coordinates to compare with.</param>
        /// <returns>true if coord1 has both components larger than coord2, false otherwise.</returns>
        public static bool operator >(Coordinates coord1, Coordinates coord2)
        {
            return coord1.x > coord2.x && coord1.y > coord2.y;
        }

        /// <summary>
        /// Compares two Coordinates, determining if a set of Coordinates is smaller than the other
        /// in both components.
        /// </summary>
        /// <param name="coord1">The Coordinates to compare.</param>
        /// <param name="coord2">The Coordinates to compare with.</param>
        /// <returns>true if coord2 has both components smaller than coord1, false otherwise.</returns>
        public static bool operator <(Coordinates coord1, Coordinates coord2)
        {
            return coord1.x < coord2.x && coord1.y < coord2.y;
        }

        /// <summary>
        /// Compares two Coordinates, determining if a set of Coordinates is greater than or equal tothe other
        /// in both components.
        /// </summary>
        /// <param name="coord1">The Coordinates to compare.</param>
        /// <param name="coord2">The Coordinates to compare with.</param>
        /// <returns>true if coord1 has both components larger than or equal to coord2, false otherwise.</returns>
        public static bool operator >=(Coordinates coord1, Coordinates coord2)
        {
            return coord1.x >= coord2.x && coord1.y >= coord2.y;
        }

        /// <summary>
        /// Compares two Coordinates, determining if a set of Coordinates is smaller than or equal to the other
        /// in both components.
        /// </summary>
        /// <param name="coord1">The Coordinates to compare.</param>
        /// <param name="coord2">The Coordinates to compare with.</param>
        /// <returns>true if coord2 has both components smaller than or equal to coord1, false otherwise.</returns>
        public static bool operator <=(Coordinates coord1, Coordinates coord2)
        {
            return coord1.x <= coord2.x && coord1.y <= coord2.y;
        }

        /// <summary>
        /// Gets the X component of these Coordinates.
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }
        }

        /// <summary>
        /// Gets the Y component of these Coordinates.
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
        }

        /// <summary>
        /// Creates a formatted string representation of these Coordinates in the format: (x, y)
        /// </summary>
        /// <returns>The string representation of these Coordinates.</returns>
        public override string ToString()
        {
            return "("+x+", "+y+")";
        }

        /// <summary>
        /// Determines if these Coordinates are equal to another set of Coordinates.
        /// </summary>
        /// <param name="obj">The Coordinates to compare to.</param>
        /// <returns>true if the Coordinates are equal.</returns>
        public bool Equals(Coordinates obj)
        {
            return this == obj;
        }

        /// <summary>
        /// Determines if these Coordinates are equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>true if these Coordinates are equal to the object.</returns>
        public override bool Equals(object obj)
        {
            return obj is Coordinates && Equals(obj);
        }

        /// <summary>
        /// Gets a hash code for these Coordinates.
        /// </summary>
        /// <returns>A hash code integer.</returns>
        public override int GetHashCode()
        {
            int hash = 23;
            hash = hash * 37 + X;
            hash = hash * 37 + Y;
            return hash;
        }
    }
}
