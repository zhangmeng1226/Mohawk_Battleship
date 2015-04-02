using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MBC.App.FormBattleship
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormBattleShip());
//            Assembly assembly = Assembly.GetExecutingAssembly();
//            foreach (String s in assembly.GetManifestResourceNames())
//            {
//                Console.WriteLine(s);
//            }
        }
    }
}
