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
        private List<GameMode> modesDesignedFor;

        public CapabilitiesAttribute(params GameMode[] modes)
        {
            modesDesignedFor = new List<GameMode>();
            foreach (var mode in modes)
            {
                modesDesignedFor.Add(mode);
            }
        }

        /// <summary>
        /// Gets a List of GameModes that a controller is compatible with.
        /// </summary>
        public List<GameMode> Capabilities
        {
            get
            {
                return modesDesignedFor;
            }
        }
    }
}
