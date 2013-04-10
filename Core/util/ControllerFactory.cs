using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace MBC.Core.util
{
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
         * <returns>A Controller loaded externally by a DLL, having the same Name property from IBattleshipOpponent as the parameter.
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
         * <summary>Loads a .DLL file and saves objects that implement the IBattleshipOpponent interface.</summary>
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
                        if (interfaces.ToString() == "Battleship.IBattleshipOpponent")
                        {
                            IBattleshipController opp = (IBattleshipController)Activator.CreateInstance(cont);
                            loadedControllers.Add(Util.ControllerToString(opp), cont);
                            break;
                        }
            }
            catch (Exception e)
            {
                Util.PrintDebugMessage("Failed to load " + fName + ": " + e.ToString());
            }
        }

        /**
         * <summary>Opens each .DLL file in the working directory's "bots" folder.</summary>
         */
        private static void LoadControllerFolder()
        {
            try
            {
                List<string> files = new List<string>(Directory.GetFiles(Util.WorkingDirectory() + "bots", "*.dll"));
                foreach (string file in files)
                    LoadControllers(file);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                Util.PrintDebugMessage("The bot directory could not be found. " + e.ToString());
            }
        }
    }
}
