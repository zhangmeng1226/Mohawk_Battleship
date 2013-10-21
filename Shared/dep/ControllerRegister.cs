using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.dep
{
    [Obsolete("No longer used.")]
    public class ControllerRegister
    {
        private Controller ctrl;

        /// <summary>
        /// Deeply copies an existing <see cref="ControllerRegister"/>.
        /// </summary>
        /// <param name="copy">A <see cref="ControllerRegister"/> to copy.</param>
        public ControllerRegister(ControllerRegister copy)
        {
            if (copy != null)
            {
                ctrl = copy.ctrl;
            }
        }

        /// <summary>
        /// Stores the <paramref name="match"/> and <paramref name="id"/> and sets the score to zero.
        /// </summary>
        /// <param name="match">The <see cref="MatchInfo"/> about a match a <see cref="Controller"/> is
        /// entered in.</param>
        /// <param name="id">The unique <see cref="ControllerID"/> of a <see cref="Controller"/> in the match.</param>
        public ControllerRegister(Controller control)
        {
            ctrl = control;
        }

        /// <summary>
        /// Gets or sets the <see cref="ControllerID"/>.
        /// </summary>
        public ControllerID ID
        {
            get
            {
                return (int)ctrl.ID;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="MatchInfo"/>.
        /// </summary>
        public MatchInfo Match
        {
            get
            {
                return new MatchInfo(ctrl.Match, ctrl.Registers.Values.ToList());
            }
        }

        /// <summary>
        /// Gets or sets the list of <see cref="ControllerID"/>s that have been designated as opponents.
        /// </summary>
        public List<ControllerID> Opponents
        {
            get
            {
                var ops = new List<ControllerID>();
                foreach (var op in ctrl.AllOpponents())
                {
                    ops.Add((int)op);
                }
                return ops;
            }
        }

        /// <summary>
        /// Gets or sets the current score.
        /// </summary>
        public int Score
        {
            get
            {
                return ctrl.MyRegister.Score;
            }
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
        /// Generates a string representation of the <see cref="ControllerRegister"/> by providing the
        /// <see cref="ControllerID"/> and <see cref="Controller"/> display name.
        /// </summary>
        /// <returns>A string representation.</returns>
        public override string ToString()
        {
            return "[" + ID + "]" + Match.ControllerNames[ID];
        }
    }
}