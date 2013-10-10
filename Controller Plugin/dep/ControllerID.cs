using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.dep
{
    [Obsolete("No longer used. Use the generally-labeled class IDNumber instead.")]
    public struct ControllerID
    {
        private int value;

        private ControllerID(int value)
        {
            this.value = value;
        }

        /// <summary>
        /// Causes an int value to act as a <see cref="IDNumber"/>.
        /// </summary>
        /// <param name="value">The int to convert.</param>
        /// <returns>A <see cref="IDNumber"/> identical to the <paramref name="value"/>.</returns>
        public static implicit operator ControllerID(int value)
        {
            return new ControllerID(value);
        }

        /// <summary>
        /// Causes a <see cref="IDNumber"/> to act as an int value.
        /// </summary>
        /// <param name="id">The <see cref="IDNumber"/> to convert.</param>
        /// <returns>An int value with the same value as the <paramref name="id"/>.</returns>
        public static implicit operator int(ControllerID id)
        {
            return id.value;
        }

        /// <summary>
        /// Generates a string that displays the value.
        /// </summary>
        /// <returns>A string representation of the value.</returns>
        public override string ToString()
        {
            return value.ToString();
        }
    }
}