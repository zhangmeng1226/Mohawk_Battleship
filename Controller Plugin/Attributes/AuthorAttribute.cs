using System;

namespace MBC.Shared.Attributes
{
    /// <summary>
    /// Provides information about the author of a <see cref="Controller"/> at the time of development.
    /// </summary>
    public class AuthorAttribute : ControllerAttribute
    {
        private string akaName;
        private string biography;
        private string firstName;
        private string lastName;

        /// <summary>
        /// Gets or sets the name that the author is also known as.
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
        /// Gets or sets a string that provides a biography about the author.
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

        /// <summary>
        /// Gets or sets the first name of the author.
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
        /// Gets or sets the last name of the author.
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
    }
}