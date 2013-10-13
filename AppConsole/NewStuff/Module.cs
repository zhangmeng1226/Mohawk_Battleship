using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.BattleshipConsole.NewStuff
{
    public abstract class Module
    {
        private List<Element> containedControls;
        private List<Module> subModules;

        public List<Element> Controls
        {
            get
            {
                return containedControls;
            }
        }

        public Module Parent
        {
            get;
            private set;
        }
    }
}