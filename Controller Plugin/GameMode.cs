using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core
{
    /// <summary>
    /// The GameMode enumeration identifies the type of game logic that will be used in a battleship round.
    /// </summary>
    public enum GameMode
    {
        /// <summary>
        /// A classic game of battleship pitted against two controllers.
        /// </summary>
        Classic,

        /// <summary>
        /// A classic game of battleship pitted against more than two controllers.
        /// </summary>
        ClassicMulti,
        
        /// <summary>
        /// A game of battleship where two controllers may make as many shots as they have remaining ships in
        /// their turn.
        /// </summary>
        Salvo,

        /// <summary>
        /// A game of battleship where more than two controllers may make as many shots as they have remaining ships
        /// in their turn.
        /// </summary>
        SalvoMulti,

        /// <summary>
        /// A game of battleship that contains items on the field that can be shot at to gain certain powered
        /// items. The controller is given a hint as to the whereabouts of the nearest item on the field.
        /// The controllers may make one shot per turn as in classic mode. Two controllers.
        /// </summary>
        Powered,

        /// <summary>
        /// A game of battleship that contains items on the field that can be shot at to gain certain powered
        /// items. The controller is given a hint as to the whereabouts of the nearest item on the field.
        /// The controllers may make one shot per turn as in classic mode. More than two controllers.
        /// </summary>
        PoweredMulti,

        /// <summary>
        /// A game of battleship where two controllers are free to fire at any time during the game. The
        /// motivation for this game mode is to make decisions on where to fire in the least amount of time
        /// to maximize shots made in a certain time.
        /// </summary>
        FreeFire,

        /// <summary>
        /// A game of battleship where two controllers are free to fire at any time during the game. The
        /// motivation for this game mode is to make decisions on where to fire in the least amount of time
        /// to maximize shots made in a certain time.
        /// </summary>
        FreeFireMulti
    }
}
