using System;

namespace MBC.Shared
{
    /// <summary>
    /// A Shot is used to identify the ControllerID of the receiver of the Shot, and provide
    /// the location of the Shot via Coordinates.
    /// </summary>
    /// <seealso cref="ControllerID"/>
    public class Shot : IEquatable<Shot>, IComparable<Shot>
    {
        private Coordinates coords;
        private ControllerID receiver;

        /// <summary>
        /// Contructs a new Shot with the same field values as another Shot.
        /// </summary>
        /// <param name="copyShot">The Shot to copy.</param>
        public Shot(Shot copyShot)
        {
            coords = copyShot.coords;
            receiver = copyShot.receiver;
        }

        /// <summary>
        /// Constructs a new Shot with Coordinates (-1, -1) and sets the sender and receiver ControllerIDs to
        /// the ones given.
        /// </summary>
        /// <param name="sender">The ControllerID of the sender.</param>
        /// <param name="receiver">The ControllerID of the receiver.</param>
        public Shot(ControllerID receiver)
        {
            this.coords = new Coordinates(-1, -1);
            this.receiver = receiver;
        }

        /// <summary>
        /// Gets or sets the ControllerID of this Shot that represents the receiver.
        /// </summary>
        public ControllerID Receiver
        {
            get
            {
                return receiver;
            }
            set
            {
                receiver = value;
            }
        }

        /// <summary>
        /// Gets or sets the Coordinates of this Shot.
        /// </summary>
        public Coordinates Coordinates
        {
            get
            {
                return coords;
            }
            set
            {
                coords = value;
            }
        }

        /// <summary>
        /// Compares the field values of two Shot objects.
        /// </summary>
        /// <param name="shot1">A Shot.</param>
        /// <param name="shot2">A Shot.</param>
        /// <returns>true if all of the fields in both Shot objects are equal.</returns>
        public static bool operator ==(Shot shot1, Shot shot2)
        {
            if (Object.ReferenceEquals(shot1, shot2))
            {
                return true;
            }
            if (Object.ReferenceEquals(shot1, null) || Object.ReferenceEquals(shot2, null))
            {
                return false;
            }
            return (shot1.Coordinates == shot2.Coordinates) && (shot1.Receiver == shot2.Receiver);
        }

        /// <summary>
        /// Does the inverse of the equality operator.
        /// </summary>
        public static bool operator !=(Shot shot1, Shot shot2)
        {
            return !(shot1 == shot2);
        }

        /// <summary>
        /// Determines the order between this Shot and another Shot.
        /// </summary>
        /// <param name="shot">The Shot to compare to.</param>
        /// <returns>1 if this Shot is ordered higher, 0 if they are the same, -1 if this one preceeds the other.</returns>
        public int CompareTo(Shot shot)
        {
            if (shot == null)
            {
                return 1;
            }

            return coords.CompareTo(shot.coords) + (receiver - shot.receiver);
        }

        /// <summary>
        /// Compares the equality of this Shot with another Shot.
        /// </summary>
        /// <param name="shot">The Shot to compare to.</param>
        /// <returns>true if all of the fields in both Shot objects are equal.</returns>
        public bool Equals(Shot shot)
        {
            return this == shot;
        }

        /// <summary>
        /// Generates a string representation of this Shot.
        /// </summary>
        /// <returns>A string representing this shot.</returns>
        public override string ToString()
        {
            return Coordinates.ToString() + "=>[" + Receiver + "]";
        }

        /// <summary>
        /// Compares the equality of this Shot with an object.
        /// </summary>
        /// <param name="shot">The object to compare to.</param>
        /// <returns>true if the object is a Shot and is equal to this Shot.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Shot);
        }

        /// <summary>
        /// Gets the has code for this Shot based on its fields.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            int hash = 23;
            hash = hash * 37 + coords.X;
            hash = hash * 37 + coords.Y;
            hash = hash * 37 + receiver;
            return hash;
        }
    }
}