using MBC.Shared;
using System;

namespace MBC.Core
{
    public class ControllerIncompatibleException : Exception
    {
        private ControllerInformation incompatible;
        private GameMode mode;

        public ControllerIncompatibleException(ControllerInformation incompatibleController, GameMode mode)
            : base(incompatibleController + " cannot be played with the " + Enum.GetName(typeof(GameMode), mode) + " game mode.")
        {
            this.incompatible = incompatibleController;
            this.mode = mode;
        }

        public ControllerInformation IncompatibleController
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