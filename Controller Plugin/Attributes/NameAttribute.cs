using System;

namespace MBC.Shared.Attributes
{
    /// <summary>
    /// Provides a name to a <see cref="Controller"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NameAttribute : Attribute
    {
        /// <summary>
        /// Gets a string that names the <see cref="Controller"/>.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Stores the <paramref name="name"/>.
        /// </summary>
        /// <param name="name">A string the identifies the <see cref="Controller"/> by name.</param>
        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}