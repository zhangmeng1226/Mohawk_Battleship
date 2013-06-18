namespace MBC.Shared
{
    /// <summary>
    /// Identical to an integer and can be used exactly as such in code. Used to identify a <see cref="Controller"/>
    /// in a specific index of arrays present in <see cref="ControllerRegister"/> and <see cref="MatchInfo"/>.
    /// <example>
    /// <code>
    /// {
    ///     var matchInfo = ...; //get the MatchInfo from somewhere
    ///     ControllerID opponent = ...; //get it from somewhere
    ///
    ///     Console.WriteLine("Playing against " + matchInfo.Players[opponent]);
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public struct ControllerID
    {
        private int value;

        private ControllerID(int value)
        {
            this.value = value;
        }

        /// <summary>
        /// Causes an int value to act as a <see cref="ControllerID"/>.
        /// </summary>
        /// <param name="value">The int to convert.</param>
        /// <returns>A <see cref="ControllerID"/> identical to the <paramref name="value"/>.</returns>
        public static implicit operator ControllerID(int value)
        {
            return new ControllerID(value);
        }

        /// <summary>
        /// Causes a <see cref="ControllerID"/> to act as an int value.
        /// </summary>
        /// <param name="id">The <see cref="ControllerID"/> to convert.</param>
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