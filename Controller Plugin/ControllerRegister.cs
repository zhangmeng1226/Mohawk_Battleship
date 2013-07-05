using System;
using System.Collections.Generic;

namespace MBC.Shared
{
    /// <summary>
    /// Provides all of the information that relates to a <see cref="Controller"/> that has been entered into
    /// a battleship game. At all times, there is one copy in the MBC core framework, and another complete copy given
    /// to an associated <see cref="Controller"/> at respectable times. Most fields may be modified by the
    /// associated <see cref="Controller"/> without causing harm to the logic in the core framework.
    /// </summary>
    public class ControllerRegister : IEquatable<ControllerRegister>
    {
        /// <summary>
        /// Deeply copies an existing <see cref="ControllerRegister"/>.
        /// </summary>
        /// <param name="copy">A <see cref="ControllerRegister"/> to copy.</param>
        public ControllerRegister(ControllerRegister copy)
        {
            if (copy != null)
            {
                Match = copy.Match;
                Score = copy.Score;
                Ships = new ShipList(copy.Ships);
                Shots = new ShotList(copy.Shots);
                ID = copy.ID;
                Opponents = new List<ControllerID>(copy.Opponents);
            }
        }

        /// <summary>
        /// Stores the <paramref name="match"/> and <paramref name="id"/> and sets the score to zero.
        /// </summary>
        /// <param name="match">The <see cref="MatchInfo"/> about a match a <see cref="Controller"/> is
        /// entered in.</param>
        /// <param name="id">The unique <see cref="ControllerID"/> of a <see cref="Controller"/> in the match.</param>
        public ControllerRegister(MatchInfo match, ControllerID id)
        {
            Match = match;
            ID = id;
            Score = 0;
        }

        /// <summary>
        /// Gets or sets the <see cref="MatchInfo"/>.
        /// </summary>
        public MatchInfo Match { get; set; }

        /// <summary>
        /// Gets or sets the current score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ShipList"/>.
        /// </summary>
        public ShipList Ships { get; set; }

        public ShipList ShipsLeft { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ShotList"/>.
        /// </summary>
        public ShotList Shots { get; set; }

        public ShotList ShotsAgainst { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ControllerID"/>.
        /// </summary>
        public ControllerID ID { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="ControllerID"/>s that have been designated as opponents.
        /// </summary>
        public List<ControllerID> Opponents { get; set; }

        /// <summary>
        /// Generates a string representation of the <see cref="ControllerRegister"/> by providing the
        /// <see cref="ControllerID"/> and <see cref="Controller"/> display name.
        /// </summary>
        /// <returns>A string representation.</returns>
        public override string ToString()
        {
            IEnumerable<string> names = Match.ControllerNames;
            return "[" + ID + "]" + Match.ControllerNames[ID];
        }

        /// <summary>
        /// Determines whether or not this <see cref="ControllerRegister"/> is equivalent to another
        /// <see cref="ControllerRegister"/>. Only the <see cref="ControllerRegister.ID"/> and <see cref="ControllerRegister.Match"/>
        /// are compared as they are only relevant for equivalence.
        /// </summary>
        /// <param name="register">The <see cref="ControllerRegister"/> to compare with.</param>
        /// <returns>A value indicating equality.</returns>
        public bool Equals(ControllerRegister register)
        {
            if (register == null)
            {
                return false;
            }
            return (ID == register.ID) && (Match == register.Match);
        }

        /// <summary>
        /// Determines whether or not this <see cref="ControllerRegister"/> is equivalent to another
        /// <see cref="ControllerRegister"/>. Only the <see cref="ControllerRegister.ID"/> and <see cref="ControllerRegister.Match"/>
        /// are compared as they are only relevant for equivalence.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>A value indicating equality.</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj as ControllerRegister);
        }

        /// <summary>
        /// Generates a hash code from the <see cref="ControllerRegister.ID"/> and
        /// <see cref="ControllerRegister.Match"/>.
        /// </summary>
        /// <returns>A hash code value.</returns>
        public override int GetHashCode()
        {
            int hash = 23;
            hash = hash * 37 + ID;
            hash = hash * 37 + Match.GetHashCode();
            return hash;
        }
    }
}