using System;

namespace MBC.Shared
{
    /// <summary>
    /// Provides all of the information that relates to a <see cref="Controller"/> that has been entered into
    /// a battleship game. At all times, there is one copy in the MBC core framework, and another complete copy given
    /// to an associated <see cref="Controller"/> at respectable times. Most fields may be modified by the
    /// associated <see cref="Controller"/> without causing harm to the logic in the core framework.
    /// </summary>
    public class Register : IEquatable<Register>
    {
        public Register(IDNumber id, string name)
        {
            ID = id;
            Name = name;
        }

        public Register(Register copy)
        {
            ID = copy.ID;
            Name = copy.Name;
            Score = copy.Score;
        }

        /// <summary>
        /// Gets or sets the <see cref="IDNumber"/>.
        /// </summary>
        public IDNumber ID { get; set; }

        public string Name
        {
            get;
            set;
        }

        public int Score
        {
            get;
            set;
        }

        /// <summary>
        /// Determines whether or not this <see cref="Register"/> is equivalent to another
        /// <see cref="Register"/>. Only the <see cref="Register.ID"/> and <see cref="Register.Match"/>
        /// are compared as they are only relevant for equivalence.
        /// </summary>
        /// <param name="register">The <see cref="Register"/> to compare with.</param>
        /// <returns>A value indicating equality.</returns>
        public bool Equals(Register register)
        {
            return (ID == register.ID) && (Name == register.Name);
        }

        /// <summary>
        /// Determines whether or not this <see cref="Register"/> is equivalent to another
        /// <see cref="Register"/>. Only the <see cref="Register.ID"/> and <see cref="Register.Match"/>
        /// are compared as they are only relevant for equivalence.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>A value indicating equality.</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Generates a hash code from the <see cref="Register.ID"/> and
        /// <see cref="Register.Match"/>.
        /// </summary>
        /// <returns>A hash code value.</returns>
        public override int GetHashCode()
        {
            return ID;
        }

        /// <summary>
        /// Generates a string representation of the <see cref="Register"/> by providing the
        /// <see cref="IDNumber"/> and <see cref="Controller"/> display name.
        /// </summary>
        /// <returns>A string representation.</returns>
        public override string ToString()
        {
            return string.Format("[%s] %s", ID, Name);
        }
    }
}