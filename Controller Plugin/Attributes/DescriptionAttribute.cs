using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Attributes
{
    /// <summary>
    /// Provides description information for a class implementing the IBattleshipController interface.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DescriptionAttribute : Attribute
    {
        public readonly string Description;

        /// <summary>
        /// Initializes this DescriptionAttribute with the given string.
        /// </summary>
        /// <param name="desc">A string used to describe a controller.</param>
        public DescriptionAttribute(string desc)
        {
            Description = desc;
        }
    }
}
