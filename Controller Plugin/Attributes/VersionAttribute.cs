using System;

namespace MBC.Shared.Attributes
{
    /// <summary>
    /// Provides the version of a <see cref="Controller"/>.
    /// </summary>
    /// <seealso cref="System.Version"/>
    [AttributeUsage(AttributeTargets.Class)]
    public class VersionAttribute : Attribute
    {
        /// <summary>
        /// Gets the <see cref="System.Version"/> number of the <see cref="Controller"/>.
        /// </summary>
        public readonly Version Version;

        /// <summary>
        /// Sets the <see cref="System.Version"/> of the <see cref="Controller"/> to <paramref name="version"/>.
        /// </summary>
        /// <param name="version">The <see cref="System.Version"/> of the <see cref="Controller"/>.</param>
        public VersionAttribute(Version version)
        {
            Version = version;
        }

        /// <summary>
        /// Sets the <see cref="System.Version"/> of the <see cref="Controller"/> from the <paramref name="verStr"/>.
        /// </summary>
        /// <param name="verStr">A string representation of a <see cref="System.Version"/>.</param>
        public VersionAttribute(string verStr)
        {
            Version = new Version(verStr);
        }

        /// <summary>
        /// Sets the <see cref="System.Version"/> of the <see cref="Controller"/> with the
        /// <paramref name="major"/> and <paramref name="minor"/> version numbers.
        /// </summary>
        /// <param name="major">The major number.</param>
        /// <param name="minor">The minior number.</param>
        public VersionAttribute(int major, int minor)
        {
            Version = new Version(major, minor);
        }

        /// <summary>
        /// Sets the <see cref="System.Version"/> of the <see cref="Controller"/> from the <paramref name="major"/>,
        /// <paramref name="minor"/>, and <paramref name="build"/> numbers.
        /// </summary>
        /// <param name="major">The major number.</param>
        /// <param name="minor">The minor number.</param>
        /// <param name="build">The build number.</param>
        public VersionAttribute(int major, int minor, int build)
        {
            Version = new Version(major, minor, build);
        }

        /// <summary>
        /// Sets the <see cref="System.Version"/> of the <see cref="Controller"/> to the given parameters.
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