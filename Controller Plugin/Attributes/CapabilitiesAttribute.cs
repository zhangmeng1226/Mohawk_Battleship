using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Attributes
{
    /// <summary>
    /// Provides the game modes that a class has been designed for.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CapabilitiesAttribute : Attribute
    {
        private readonly List<GameMode> Capabilities;

        public CapabilitiesAttribute(params GameMode[] modes)
        {
            Capabilities = new List<GameMode>();
            foreach (var mode in modes)
            {
                Capabilities.Add(mode);
            }
        }

        /// <summary>
        /// Checks whether the given GameMode is compatible with a controller.
        /// </summary>
        /// <param name="mode">The GameMode to check</param>
        /// <returns>true if the GameMode is compatible, false otherwise.</returns>
        public bool CompatibleWith(GameMode mode)
        {
            foreach (var givenModes in Capabilities)
            {
                if ((mode & givenModes) != givenModes)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
