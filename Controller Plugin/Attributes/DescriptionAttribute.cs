using System;

namespace MBC.Shared.Attributes
{
    /// <summary>
    /// Provides a description of a <see cref="Controller"/>.
    /// </summary>
    public class DescriptionAttribute : ControllerAttribute
    {
        /// <summary>
        /// Gets a string that describes the <see cref="Controller"/>.
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// Stores the <paramref name="description"/>.
        /// </summary>
        /// <param name="description">A string used to describe a <see cref="Controller"/></param>
        public DescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}