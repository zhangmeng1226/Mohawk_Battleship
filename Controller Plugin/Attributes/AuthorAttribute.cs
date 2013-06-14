using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Attributes
{
    /// <summary>
    /// Provides author information about a class implementing the IBattleshipController interface.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthorAttribute : Attribute
    {
        private string firstName;
        private string lastName;

        private string akaName;
        private string biography;

        /// <summary>
        /// Gets or sets the first name of the Author.
        /// </summary>
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
            }
        }

        /// <summary>
        /// Gets or sets the last name of the Author.
        /// </summary>
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name that the Author is also known as.
        /// </summary>
        public string AKAName
        {
            get
            {
                return akaName;
            }
            set
            {
                akaName = value;
            }
        }

        /// <summary>
        /// Gets or sets a string that provides a biography about the Author.
        /// </summary>
        public string Biography
        {
            get
            {
                return biography;
            }
            set
            {
                biography = value;
            }
        }
    }
}
