using MBC.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Util
{
    /// <summary>
    /// Provides the base type of battleship controller that attempts to ease development of a
    /// derived controller.
    /// </summary>
    public abstract class Controller2 : IController2
    {
        private Player plr;

        /// <summary>
        /// The Player object that is assigned to the controller when added to a match.
        /// </summary>
        public Player Player
        {
            get
            {
                return plr;
            }
            set
            {
                plr = value;
                Initialize();
            }
        }

        /// <summary>
        /// Invoked when the controller is to initialize.
        /// </summary>
        protected abstract void Initialize();
    }
}