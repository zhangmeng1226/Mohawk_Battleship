using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Attributes
{
    /// <summary>
    /// Provides the game modes that a class has been designed for.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CapabilitiesAttribute : Attribute
    {
        private readonly GameMode Capabilities;

        public CapabilitiesAttribute(params GameMode[] modes)
        {
            foreach (var mode in modes)
            {
                Capabilities |= mode;
            }
        }

        /// <summary>
        /// Checks whether the given GameMode is compatible with a controller.
        /// </summary>
        /// <param name="mode">The GameMode to check</param>
        /// <returns>true if the GameMode is compatible, false otherwise.</returns>
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
