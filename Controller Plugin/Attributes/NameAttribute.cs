using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Attributes
{
    /// <summary>
    /// Provides the name of a controller class implementing the IBattleshipController interface.
    /// This attribute is mandatory for a controller to be loaded.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NameAttribute : Attribute
    {
        public readonly string Name;

        /// <summary>
        /// Initializes this NameAttribute with the given name string.
        /// </summary>
        /// <param name="name">The name of the controller.</param>
        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}
