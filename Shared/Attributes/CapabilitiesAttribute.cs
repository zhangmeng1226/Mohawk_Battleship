using System;

namespace MBC.Shared.Attributes
{
    /// <summary>
    /// Provides information about the various <see cref="GameMode"/>s that a <see cref="Controller"/> has been
    /// designed for. Causes a <see cref="Controller"/> to be loaded only in the given <see cref="GameMode"/>s.
    /// </summary>
    public class CapabilitiesAttribute : ControllerAttribute
    {
        private readonly GameMode Capabilities;

        /// <summary>
        /// Sets the <see cref="GameMode"/>s compatible to the <paramref name="modes"/> provided.
        /// </summary>
        /// <param name="modes">A variable number of <see cref="GameMode"/>s.</param>
        public CapabilitiesAttribute(params GameMode[] modes)
        {
            foreach (var mode in modes)
            {
                Capabilities |= mode;
            }
        }

        /// <summary>
        /// Checks if the <see cref="Controller"/> is compatible with the <paramref name="mode"/>
        /// </summary>
        /// <param name="mode">The <see cref="GameMode"/> to check.</param>
        /// <returns>A value that indicates if a <see cref="Controller"/> is capable with the <paramref name="mode"/>.</returns>
        public bool CompatibleWith(GameMode mode)
        {
            foreach (var gameMode in Enum.GetValues(typeof(GameMode)))
            {
                if (mode.HasFlag((GameMode)gameMode) && !Capabilities.HasFlag((GameMode)gameMode))
                {
                    return false;
                }
            }
            return true;
        }
    }
}