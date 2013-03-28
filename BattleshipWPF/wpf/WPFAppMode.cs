using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Battleship
{
    public class WPFAppMode : ProgramMode
    {
        Application wpfApplication;

        public WPFAppMode()
        {
            wpfApplication = new Application();
        }

        public void Start()
        {
            wpfApplication.Run(new MainWindow());
        }
    }
}
