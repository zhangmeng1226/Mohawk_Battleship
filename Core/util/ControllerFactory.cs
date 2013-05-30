using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace MBC.Core.util
{
    /**
     * <summary>The ControllerFactory is a purely static class that deals with loading the IBattleshipControllers
     * from files.
     * 
     * Using this class is simple by utilizing 2 methods in this class:
     * 
     *      LoadControllerFolder() - Loads the classes that implement the IBattleshipController interface from files.
     *                               Generally, this method should be invoked when updating a list of IBattleshipControllers.
     *      CreateController(string) - Constructs an object out of a class implementing IBattleshipController that has been loaded.
     *                                 The string specifies the name of the loaded IBattleshipController.
     *      
     * A list of loaded controllers can be retrieved from the Names property. The strings in the list can then
     * be used to construct a new controller via CreateController(string).
     * 
     * Use of LoadControllers(string) is only required when loading a file from outside the default directory.
     * </summary>
     */
    public class ControllerFactory
    {
        private ControllerFactory() { }

        private static Dictionary<string, Type> loadedControllers = new Dictionary<string, Type>();

        /**
         * <summary>Returns a list of strings representing the name and version of each controller</summary>
         */
        public static List<string> Names
        {
            get
            {
                return loadedControllers.Keys.ToList();
            }
        }

        /**
         * <summary>Returns the object that is identified by name.</summary>
         * <returns>A Controller loaded externally by a DLL, having the same Name property from IBattleshipController as the parameter.
         * If the class was not found, then this will return null.</returns>
         */
        public static IBattleshipController CreateController(string name)
        {
            Type result = null;
            loadedControllers.TryGetValue(name, out result);
            if (result == null)
                return null;
            return (IBattleshipController)Activator.CreateInstance(result);
        }

        /**
         * <summary>Loads a .DLL file and saves objects that implement the IBattleshipController interface.</summary>
         * <param name="fName">The absolute pathname to the .DLL file</param>
         * <remarks>It is currently possible to load more than one controller from a single DLL.</remarks>
         */
        public static void LoadControllers(string fName)
        {
            try
            {
                Assembly dllInfo = Assembly.LoadFile(fName);
                Type[] types = dllInfo.GetTypes();

                foreach (Type cont in types)
                    foreach (Type interfaces in cont.GetInterfaces())
                    {
                        if (interfaces.ToString() == "MBC.Core.IBattleshipController")
                        {
                            IBattleshipController opp = (IBattleshipController)Activator.CreateInstance(cont);
                            loadedControllers[Util.ControllerToString(opp)] = cont;
                            break;
                        }
                    }
            }
            catch (Exception e)
            {
                Util.PrintDebugMessage("Failed to load " + fName + ": " + e.ToString());
            }
        }

        /**
         * <summary>Opens each .DLL file in the working directory's "bots" folder. Overwrites
         * existing Controller objects with new ones loaded.</summary>
         */
        public static void LoadControllerFolder()
        {
            try
            {
                List<string> files = new List<string>(Directory.GetFiles(Util.WorkingDirectory() + "bots", "*.dll"));
                foreach (string file in files)
                {
                    LoadControllers(file);
                }
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                Util.PrintDebugMessage("The bot directory could not be found. " + e.ToString());
            }
        }
    }
}
