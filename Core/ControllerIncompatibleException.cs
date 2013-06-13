using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core
{
    public class ControllerIncompatibleException : Exception
    {
        private Controller.ClassInfo incompatible;
        private GameMode mode;

        public ControllerIncompatibleException(Controller.ClassInfo incompatibleController, GameMode mode)
            : base(incompatibleController+" cannot be played with the "+Enum.GetName(typeof(GameMode), mode)+" game mode.")
        {
            this.incompatible = incompatibleController;
            this.mode = mode;
        }

        public Controller.ClassInfo IncompatibleController
        {
            get
            {
                return incompatible;
            }
        }

        public GameMode IncompatibleMode
        {
            get
            {
                return mode;
            }
        }
    }
}
