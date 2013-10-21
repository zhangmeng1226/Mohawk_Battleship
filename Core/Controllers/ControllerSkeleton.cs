using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MBC.Shared;
using MBC.Shared.Attributes;

namespace MBC.Core
{
    /// <summary>
    /// Provides various information about a <see cref="Controller"/> that is loaded from an external library via
    /// <see cref="ControllerSkeleton.LoadControllerFolder(string)"/>. Contains the <see cref="Type"/>
    /// that represents a constructable <see cref="Controller"/>.
    /// </summary>
    public class ControllerSkeleton
    {
        private Dictionary<Type, ControllerAttribute> attributes;
        private Type controllerClass;
        private string dllFile;

        private ControllerSkeleton()
        {
            attributes = new Dictionary<Type, ControllerAttribute>();
        }

        /// <summary>
        /// Gets the <see cref="Type"/> that is used to create the <see cref="Controller"/>.
        /// </summary>
        public Type Controller
        {
            get
            {
                return controllerClass;
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
        /// Loads a list of <see cref="ControllerSkeleton"/> from a single .DLL file.
        /// </summary>
        /// <param name="filePath">The absolute path to the .DLL file.</param>
        /// <returns>A list of successfully loaded <see cref="ControllerSkeleton"/>s.</returns>
        public static List<ControllerSkeleton> LoadControllerDLL(string filePath)
        {
            var results = new List<ControllerSkeleton>();
            try
            {
                var dllInfo = Assembly.LoadFile(filePath);
                var types = dllInfo.GetTypes();

                foreach (Type cont in types)
                {
                    if (cont.GetInterface("IController", false) != null && !cont.IsAbstract && cont.GetConstructor(Type.EmptyTypes) != null)
                    {
                        string[] pathSplit = filePath.Split('\\');

                        ControllerSkeleton info = new ControllerSkeleton();
                        info.controllerClass = cont;
                        info.dllFile = pathSplit[pathSplit.Count() - 1];
                        foreach (var attribute in cont.GetCustomAttributes(false))
                        {
                            if (attribute is ControllerAttribute)
                            {
                                info.attributes[attribute.GetType()] = (ControllerAttribute)attribute;
                            }
                        }
                        if (info.GetAttribute<NameAttribute>() == null)
                        {
                            info.attributes[typeof(NameAttribute)] = new NameAttribute("undefined");
                        }
                        if (info.GetAttribute<VersionAttribute>() == null)
                        {
                            info.attributes[typeof(VersionAttribute)] = new VersionAttribute(0, 0);
                        }
                        if (info.GetAttribute<CapabilitiesAttribute>() == null)
                        {
                            info.attributes[typeof(CapabilitiesAttribute)] = new CapabilitiesAttribute(GameMode.Classic);
                        }
                        results.Add(info);
                    }
                }
            }
            catch
            {
            }
            return results;
        }

        /// <summary>
        /// Searches a given folder for dynamic-loaded libraries (.dll) and attempts to load <see cref="Controller"/>s
        /// from them. Creates <see cref="ControllerSkeleton"/> objects for each unique <see cref="Controller"/>.
        /// </summary>
        /// <param name="path">The absolute path name to a folder containing DLL files.</param>
        /// <exception cref="DirectoryNotFoundException">The given directory was not found or was a relative path.</exception>
        /// <returns>A list of <see cref="ControllerSkeleton"/> objects that have been created from
        /// findings.</returns>
        public static List<ControllerSkeleton> LoadControllerFolder(string path)
        {
            var results = new List<ControllerSkeleton>();
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

        public Controller CreateInstance()
        {
            return (Controller)Activator.CreateInstance(Controller);
        }

        public T GetAttribute<T>()
        {
            if (attributes.ContainsKey(typeof(T)))
            {
                return (T)((object)attributes[typeof(T)]);
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Generates a string that may be used as a display name, from the name and version.
        /// </summary>
        public override string ToString()
        {
            return GetAttribute<NameAttribute>() + " v(" + GetAttribute<VersionAttribute>() + ")";
        }
    }
}