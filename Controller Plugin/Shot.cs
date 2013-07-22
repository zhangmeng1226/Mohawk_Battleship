using System;

namespace MBC.Shared
{
    /// <summary>
    /// Provides information about a primary component of a battleship game match; the shot. Contains
    /// the <see cref="Coordinates"/> used to target a <see cref="Ship"/> of a <see cref="Controller"/> associated
    /// with a certain <see cref="ControllerID"/>.
    /// </summary>
    public class Shot : IEquatable<Shot>, IComparable<Shot>
    {
        private Coordinates coords;
        private ControllerID receiver;

        /// <summary>
        /// Copies an existing <see cref="Shot"/>.
        /// </summary>
        /// <param name="copyShot">The <see cref="Shot"/> to copy.</param>
        public Shot(Shot copyShot)
        {
            coords = copyShot.coords;
            receiver = copyShot.receiver;
        }

        /// <summary>
        /// Initializes the <see cref="Shot.Coordinates"/> to (-1, -1) and stores the <paramref name="receiver"/>.
        /// </summary>
        /// <param name="receiver">The receiving <see cref="ControllerRegister"/> of this <see cref="Shot"/>.</param>
        public Shot(ControllerID receiver)
        {
            this.coords = new Coordinates(-1, -1);
            this.receiver = receiver;
        }

        private Shot() { }

        /// <summary>
        /// Gets or sets the <see cref="ControllerID"/> that identifies the <see cref="ControllerRegister"/>
        /// receiving this <see cref="Shot"/>.
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
        /// Gets or sets the <see cref="Coordinates"/>.
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
        /// Compares the field values of two <see cref="Shot"/>s.
        /// </summary>
        /// <param name="shot1">A <see cref="Shot"/>.</param>
        /// <param name="shot2">A <see cref="Shot"/>.</param>
        /// <returns>true if all of the fields in both <see cref="Shot"/>s are equal.</returns>
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
        /// Compares the field values of two <see cref="Shot"/>s.
        /// </summary>
        /// <param name="shot1">A <see cref="Shot"/>.</param>
        /// <param name="shot2">A <see cref="Shot"/>.</param>
        /// <returns>true if one or more of the fields in both <see cref="Shot"/>s are inequal.</returns>
        public static bool operator !=(Shot shot1, Shot shot2)
        {
            return !(shot1 == shot2);
        }

        /// <summary>
        /// Determines the order between this <see cref="Shot"/> and another <see cref="Shot"/>.
        /// </summary>
        /// <param name="shot">The <see cref="Shot"/> to compare to.</param>
        /// <returns>1 if this <see cref="Shot"/> is ordered higher, 0 if they are the same, -1 if this one preceeds the other.</returns>
        public int CompareTo(Shot shot)
        {
            if (shot == null)
            {
                return 1;
            }

            return coords.CompareTo(shot.coords) + (receiver - shot.receiver);
        }

        /// <summary>
        /// Compares the equality of this <see cref="Shot"/> with another <see cref="Shot"/>.
        /// </summary>
        /// <param name="shot">The <see cref="Shot"/> to compare to.</param>
        /// <returns>true if all of the fields in both <see cref="Shot"/>s are equal.</returns>
        public bool Equals(Shot shot)
        {
            return this == shot;
        }

        /// <summary>
        /// Generates a string representation of this <see cref="Shot"/>.
        /// </summary>
        /// <returns>A string representing this <see cref="Shot"/>.</returns>
        public override string ToString()
        {
            return Coordinates.ToString() + "=>[" + Receiver + "]";
        }

        /// <summary>
        /// Compares the equality of this <see cref="Shot"/> with an object.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>true if the object is a <see cref="Shot"/> and is equal to this <see cref="Shot"/>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Shot);
        }

        /// <summary>
        /// Gets the has code for this <see cref="Shot"/> based on its fields.
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