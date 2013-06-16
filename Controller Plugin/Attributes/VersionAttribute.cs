using System;

namespace MBC.Shared.Attributes
{
    /// <summary>
    /// Provides version information for a class implementing an IBattleshipController interface.
    /// This attribute is mandatory for a controller to be loaded.
    /// </summary>
    /// <seealso cref="System.Version"/>
    [AttributeUsage(AttributeTargets.Class)]
    public class VersionAttribute : Attribute
    {
        public readonly Version Version;

        /// <summary>
        /// Initializes this VersionAttribute with an existing Version object.
        /// </summary>
        /// <param name="ver">The Version object to set to.</param>
        public VersionAttribute(Version ver)
        {
            Version = ver;
        }

        /// <summary>
        /// Initializes this VersionAttribute with the specified string.
        /// </summary>
        /// <param name="ver">A string representing the version.</param>
        public VersionAttribute(string ver)
        {
            Version = new Version(ver);
        }

        /// <summary>
        /// Initializes this VersionAttribute with the specified major and minor values.
        /// </summary>
        /// <param name="major">The major number.</param>
        /// <param name="minor">The minior number.</param>
        public VersionAttribute(int major, int minor)
        {
            Version = new Version(major, minor);
        }

        /// <summary>
        /// Initializes this VersionAttribute with the specified major, minor, and build values.
        /// </summary>
        /// <param name="major">The major number.</param>
        /// <param name="minor">The minor number.</param>
        /// <param name="build">The build number.</param>
        public VersionAttribute(int major, int minor, int build)
        {
            Version = new Version(major, minor, build);
        }

        /// <summary>
        /// Initializes this VersionAttribute with the specified major, minor, build, and revision numbers.
        /// </summary>
        /// <param name="major">The major number.</param>
        /// <param name="minor">The minor number.</param>
        /// <param name="build">The build number.</param>
        /// <param name="revision">The revision number.</param>
        public VersionAttribute(int major, int minor, int build, int revision)
        {
            Version = new Version(major, minor, build, revision);
        }
    }
}