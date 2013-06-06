using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace MBC.Core.util
{
    /// <summary>The ControllerFactory is a purely static class that deals with loading IBattleshipController objects
    /// from files.
    /// 
    /// Using this class is simple by utilizing 2 methods in this class:
    /// <list type="bullet">
    ///      <item>LoadControllerFolder() - Loads the classes that implement the IBattleshipController interface from files.
    ///                               Generally, this method should be invoked when updating a list of IBattleshipControllers.</item>
    ///      <item>CreateController() - Constructs an object out of a class implementing IBattleshipController that has been loaded.
    ///                                 The string specifies the name of the loaded IBattleshipController.</item>
    /// </list>
    /// A list of loaded controllers can be retrieved from the Names property. The strings in the list can then
    /// be used to construct a new controller via CreateController().
    /// 
    /// Use of LoadControllers() is only required when loading a file from outside the default directory.
    /// </summary>
    public class ControllerFactory
    {
        private ControllerFactory() { }

        private static Dictionary<string, Type> loadedControllers = new Dictionary<string, Type>();

        /// <summary>Returns a list of strings representing the name and version of each controller</summary>
        public static List<string> Names
        {
            get
            {
                return loadedControllers.Keys.ToList();
            }
        }

        /// <summary>Returns the object that is identified by name.</summary>
        /// <param name="name">The string that identifies the IBattleshipController to create. See <see cref="Names"/></param>
        /// <returns>A Controller loaded externally by a DLL, having the same Name property from IBattleshipController as the parameter.
        /// If the class was not found, then this will return null.</returns>
        public static IBattleshipController CreateController(string name)
        {
            Type result = null;
            loadedControllers.TryGetValue(name, out result);
            if (result == null)
            {
                return null;
            }
            return (IBattleshipController)Activator.CreateInstance(result);
        }

        /// <summary>
        /// Loads a .DLL file and saves the types that implement the IBattleshipController interface.
        /// It is possible to load more than one controller from a single DLL.
        /// </summary>
        /// <param name="fName">The absolute pathname to the .DLL file</param>
        public static void LoadControllers(string fName)
        {
            try
            {
                var dllInfo = Assembly.LoadFile(fName);
                var types = dllInfo.GetTypes();

                foreach (Type cont in types)
                {
                    foreach (Type interfaces in cont.GetInterfaces())
                    {
                        if (interfaces.ToString() == "MBC.Core.IBattleshipController")
                        {
                            var opp = (IBattleshipController)Activator.CreateInstance(cont);
                            loadedControllers[Utility.ControllerToString(opp)] = cont;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utility.PrintDebugMessage("Failed to load " + fName + ": " + e.ToString());
            }
        }

        /// <summary>Opens each .DLL file in the working directory's "bots" folder. Overwrites
        /// existing controller objects with new ones loaded.</summary>
        public static void LoadControllerFolder()
        {
            try
            {
                var files = new List<string>(Directory.GetFiles(Utility.WorkingDirectory() + "bots", "*.dll"));
                foreach (string file in files)
                {
                    LoadControllers(file);
                }
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                Utility.PrintDebugMessage("The bot directory could not be found. " + e.ToString());
            }
        }
    }
}
