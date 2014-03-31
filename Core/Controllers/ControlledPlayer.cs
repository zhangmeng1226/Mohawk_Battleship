using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Controllers
{
    public class ControlledPlayer : Player
    {
        /// <summary>
        /// Constructs a Player with an ID and a name.
        /// </summary>
        /// <param name="newId">The ID number in the match</param>
        /// <param name="newName">The name of the player</param>
        public ControlledPlayer(IDNumber newId, string newName, IController2 cont)
            : base(newId, newName)
        {
            Controller = cont;
        }

        protected internal IController2 Controller
        {
            get;
            set;
        }
    }
}