namespace MBC.Shared
{
    /// Provides various behaviours as to how a Match will handle the number of rounds it is configured
    /// with.
    /// </summary>
    public enum RoundMode
    {
        /// <summary>
        /// Creates and plays through <see cref="Round"/>s until the number of <see cref="Round"/>s generated
        /// is equal to the number of rounds configured.
        /// </summary>
        /// <seealso cref="Round"/>
        AllRounds,

        /// <summary>
        /// Creates and plays through <see cref="Round"/>s until a <see cref="Register"/> has
        /// reached the number of rounds configured.
        /// </summary>
        /// <seealso cref="Round"/>
        /// <seealso cref="Register"/>
        FirstTo
    }
}