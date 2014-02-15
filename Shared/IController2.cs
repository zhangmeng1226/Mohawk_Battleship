using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared
{
    public interface IController2
    {
        event StringOutputHandler ControllerMessageEvent;

        Match Match { get; set; }

        /// <summary>
        /// Called when required to create and return a <see cref="Shot"/>. Refer to the rules of the
        /// <see cref="MatchConfig.GameMode"/> in the <see cref="Controller.Register"/> when creating the
        /// <see cref="Shot"/>.
        /// </summary>
        /// <returns>A <see cref="Shot"/> to be processed by the MBC core framework.</returns>
        Shot MakeShot();

        /// <summary>
        /// Called when the <see cref="Register.Ships"/> in the <see cref="Controller.Register"/> must
        /// be placed. Refer to the rules of the <see cref="MatchConfig.GameMode"/> in the <see cref="Controller.Register"/>.
        /// </summary>
        IList<Ship> PlaceShips();
    }
}