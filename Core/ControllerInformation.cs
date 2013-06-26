using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MBC.Core
{
    /// <summary>
    /// Provides various information about a <see cref="Controller"/> that is loaded from an external library via
    /// <see cref="ControllerInformation.LoadControllerFolder(string)"/>. Contains the <see cref="Type"/>
    /// that represents a constructable <see cref="Controller"/>.
    /// </summary>
    public class ControllerInformation
    {
        private AcademicInfoAttribute academicAttrib;
        private AuthorAttribute authorAttrib;
        private CapabilitiesAttribute capableAttrib;
        private Type controllerInterface;
        private DescriptionAttribute descAttrib;
        private string dllFile;
        private NameAttribute nameAttrib;
        private VersionAttribute verAttrib;

        /// <summary>
        /// Copies the given parameters to the internal members.
        /// </summary>
        /// <param name="name">The <see cref="NameAttribute"/> set on a <see cref="Controller"/>.</param>
        /// <param name="ver">The <see cref="VersionAttribute"/> se on a <see cref="Controller"/>.</param>
        /// <param name="desc">The <see cref="DescriptionAttribute"/> set on a <see cref="Controller"/>.</param>
        /// <param name="auth">The <see cref="AuthorAttribute"/> set on a <see cref="Controller"/>.</param>
        /// <param name="academic">The <see cref="AcademicInfoAttribute"/> set on a <see cref="Controller"/>.</param>
        /// <param name="capabilities">The <see cref="CapabilitiesAttribute"/> set on a
        /// <see cref="Controller"/>.</param>
        /// <param name="dll">A string of the absolute path to the library file the <see cref="Controller"/>
        /// was loaded from.</param>
        /// <param name="inter">The <see cref="Type"/> that is used to construct <see cref="Controller"/>s</param>.
        public ControllerInformation(NameAttribute name, VersionAttribute ver, DescriptionAttribute desc,
            AuthorAttribute auth, AcademicInfoAttribute academic, CapabilitiesAttribute capabilities,
            string dll, Type inter)
        {
            this.nameAttrib = name;
            this.verAttrib = ver;
            this.descAttrib = desc;
            this.authorAttrib = auth;
            this.academicAttrib = academic;
            this.capableAttrib = capabilities;
            this.dllFile = dll;
            this.controllerInterface = inter;
        }

        /// <summary>
        /// Gets the <see cref="AcademicInfoAttribute"/>.
        /// </summary>
        /// <seealso cref="AcademicInfoAttribute"/>
        public AcademicInfoAttribute AcademicInfo
        {
            get
            {
                return academicAttrib;
            }
        }

        /// <summary>
        /// Gets the <see cref="AuthorAttribute"/>.
        /// </summary>
        /// <seealso cref="AuthorAttribute"/>
        public AuthorAttribute AuthorInfo
        {
            get
            {
                return authorAttrib;
            }
        }

        /// <summary>
        /// Gets the <see cref="CapabilitiesAttribute"/>.
        /// </summary>
        public CapabilitiesAttribute Capabilities
        {
            get
            {
                return capableAttrib;
            }
        }

        /// <summary>
        /// Gets the <see cref="Type"/> that is used to create the <see cref="Controller"/>.
        /// </summary>
        public Type Controller
        {
            get
            {
                return controllerInterface;
            }
        }

        /// <summary>
        /// Gets a string containing the description.
        /// </summary>
        public string Description
        {
            get
            {
                return descAttrib.Description;
            }
        }

        /// <summary>
        /// Gets the absolute path to the originating library file.
        /// </summary>
        public string DLLFileName
        {
            get
            {
                return dllFile;
            }
        }

        /// <summary>
        /// Gets a string representing the name.
        /// </summary>
        public string Name
        {
            get
            {
                return nameAttrib.Name;
            }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public Version Version
        {
            get
            {
                return verAttrib.Version;
            }
        }
        /// <summary>
        /// Searches a given folder for dynamic-loaded libraries (.dll) and attempts to load <see cref="Controller"/>s
        /// from them. Creates <see cref="ControllerInformation"/> objects for each unique <see cref="Controller"/>.
        /// </summary>
        /// <param name="path">The absolute path name to a folder containing DLL files.</param>
        /// <exception cref="DirectoryNotFoundException">The given directory was not found or was a relative path.</exception>
        /// <returns>A list of <see cref="ControllerInformation"/> objects that have been created from
        /// findings.</returns>
        public static List<ControllerInformation> LoadControllerFolder(string path)
        {
            var results = new List<ControllerInformation>();
            try
            {
                var filePaths = new List<string>(Directory.GetFiles(path, "*.dll"));

                foreach (var file in filePaths)
                {
                    results.AddRange(LoadControllerDLL(file));
                }
            }
            catch { }
            return results;
        }

        /// <summary>
        /// Loads a list of <see cref="ControllerInformation"/> from a single .DLL file.
        /// </summary>
        /// <param name="filePath">The absolute path to the .DLL file.</param>
        /// <returns>A list of successfully loaded <see cref="ControllerInformation"/>s.</returns>
        public static List<ControllerInformation> LoadControllerDLL(string filePath)
        {
            var results = new List<ControllerInformation>();
            try
            {
                var dllInfo = Assembly.LoadFile(filePath);
                var types = dllInfo.GetTypes();

                foreach (Type cont in types)
                {
                    //Iterating through each class in this assembly.
                    if (cont.IsSubclassOf(typeof(Controller)))
                    {
                        NameAttribute nameAttrib = (NameAttribute)cont.GetCustomAttributes(typeof(NameAttribute), false)[0];
                        VersionAttribute verAttrib = (VersionAttribute)cont.GetCustomAttributes(typeof(VersionAttribute), false)[0];
                        CapabilitiesAttribute capAttrib = (CapabilitiesAttribute)cont.GetCustomAttributes(typeof(CapabilitiesAttribute), false)[0];
                        if (nameAttrib != null && verAttrib != null && capAttrib != null)
                        {
                            //Split the absolute path. We only want the name of the DLL file.
                            string[] pathSplit = filePath.Split('\\');

                            ControllerInformation info = new ControllerInformation(nameAttrib, verAttrib,
                                (DescriptionAttribute)cont.GetCustomAttributes(typeof(DescriptionAttribute), false)[0],
                                (AuthorAttribute)cont.GetCustomAttributes(typeof(AuthorAttribute), false)[0],
                                (AcademicInfoAttribute)cont.GetCustomAttributes(typeof(AcademicInfoAttribute), false)[0],
                                capAttrib,
                                pathSplit[pathSplit.Count() - 1],
                                cont);
                            results.Add(info);
                        }
                        break;
                    }
                }
            }
            catch
            {
                //Unable to load a .DLL file; we don't care about this assembly.
            }
            return results;
        }

        /// <summary>
        /// Generates a string that may be used as a display name, from the name and version.
        /// </summary>
        public override string ToString()
        {
            return Name + " v(" + Version.ToString() + ")";
        }
    }
}