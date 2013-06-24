using MBC.Shared;
using System;

namespace MBC.Core
{
    /// <summary>
    /// An exception that provides information about a <see cref="Controller"/>
    /// specified in a <see cref="ControllerInformation"/> that reveals its incompatibility with
    /// a certain <see cref="GameMode"/>.
    /// </summary>
    public class ControllerIncompatibleException : Exception
    {
        private ControllerInformation incompatible;
        private GameMode mode;

        /// <summary>
        /// Attaches the <see cref="ControllerInformation"/> that is incompatible with the given
        /// <see cref="GameMode"/>.
        /// </summary>
        /// <param name="incompatibleController">The incompatible <see cref="ControllerInformation"/>.</param>
        /// <param name="mode">The <see cref="GameMode"/> incompatible with the <see cref="ControllerInformation"/>.</param>
        public ControllerIncompatibleException(ControllerInformation incompatibleController, GameMode mode)
            : base(incompatibleController + " cannot be played with the " + Enum.GetName(typeof(GameMode), mode) + " game mode.")
        {
            this.incompatible = incompatibleController;
            this.mode = mode;
        }

        /// <summary>
        /// Gets the <see cref="ControllerInformation"/> that is incompatible.
        /// </summary>
        public ControllerInformation IncompatibleController
        {
            get
            {
                return incompatible;
            }
        }

        /// <summary>
        /// Gets the <see cref="GameMode"/> that is incompatible.
        /// </summary>
        public GameMode IncompatibleMode
        {
            get
            {
                return mode;
            }
        }
    }
}