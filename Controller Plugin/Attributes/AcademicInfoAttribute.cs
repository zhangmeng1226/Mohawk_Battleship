using System;

namespace MBC.Shared.Attributes
{
    /// <summary>
    /// Provides information about the academic information of a <see cref="Controller"/>'s developer at the
    /// time of development.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AcademicInfoAttribute : Attribute
    {
        /// <summary>
        /// The program year of the student.
        /// </summary>
        public readonly int CurrentYear;

        /// <summary>
        /// The institution the student has attended.
        /// </summary>
        public readonly string Institution;

        /// <summary>
        /// The program the student has been enrolled in.
        /// </summary>
        public readonly string Program;
        /// <summary>
        /// Stores all of the parameters.
        /// </summary>
        /// <param name="school">Sets the <see cref="AcademicInfoAttribute.Institution"/>.</param>
        /// <param name="program">Sets the <see cref="AcademicInfoAttribute.Program"/>.</param>
        /// <param name="year">Sets the <see cref="AcademicInfoAttribute.CurrentYear"/>.</param>
        public AcademicInfoAttribute(string school, string program, int year)
        {
            this.Institution = school;
            this.Program = program;
            this.CurrentYear = year;
        }
    }
}