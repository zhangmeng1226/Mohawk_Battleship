namespace MBC.Core.Accolades
{
    /// <summary>
    /// The Accolade class is the base class for a number of classes that are used to describe a Round from
    /// various events that occur within the Round.
    /// </summary>
    public abstract class Accolade
    {
        /// <summary>
        /// Gets a string that describes why this Accolade was generated and added.
        /// </summary>
        public abstract string Message
        {
            get;
        }

        /// <summary>
        /// Gets a string that identifies the name of this Accolade.
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Gets a string that describes this Accolade.
        /// </summary>
        public abstract string Description
        {
            get;
        }
    }
}