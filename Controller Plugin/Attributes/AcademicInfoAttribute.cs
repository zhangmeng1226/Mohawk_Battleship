using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Attributes
{
    /// <summary>
    /// Allows a controller to provide information about it's developer's academic information.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AcademicInfoAttribute : Attribute
    {
        public readonly string Institution;
        public readonly string Program;
        public readonly int CurrentYear;

        public AcademicInfoAttribute(string school, string program, int year)
        {
            this.Institution = school;
            this.Program = program;
            this.CurrentYear = year;
        }
    }
}
