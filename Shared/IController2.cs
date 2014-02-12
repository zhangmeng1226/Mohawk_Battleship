using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared
{
    public interface IController2
    {
        event StringOutputHandler ControllerMessageEvent;

        Match match { get; set; }

        /// <summary>
        /// Called when required to create and return a <see cref="Shot"/>. Refer to the rules of the
        /// <see cref="MatchConfig.GameMode"/> in the <see cref="Controller.Register"/> when creating the
        /// <see cref="Shot"/>.
        /// </summary>
        /// <returns>A <see cref="Shot"/> to be processed by the MBC core framework.</returns>
        Shot MakeShot();

        /// <summary>
        /// Called when an opposing <see cref="Controller"/> in the same match has created a
        /// <see cref="Shot"/> against this <see cref="Controller"/>.
        /// </summary>
        /// <param name="shot">The <see cref="Shot"/> made by an opposing <see cref="Controller"/></param>
        void OpponentShot(Shot shot);

        /// <summary>
        /// Called when the <see cref="Register.Ships"/> in the <see cref="Controller.Register"/> must
        /// be placed. Refer to the rules of the <see cref="MatchConfig.GameMode"/> in the <see cref="Controller.Register"/>.
        /// </summary>
        IList<Ship> PlaceShips();

        /// <summary>
        /// Called when the round has been lost.
        /// </summary>
        void RoundLost();

        /// <summary>
        /// Called when the round has been won.
        /// </summary>
        void RoundWon();

        /// <summary>
        /// Called when a <paramref name="shot"/> created earlier hit an opposing <see cref="Controller"/>'s
        /// <see cref="Ship"/>. Indicates via <paramref name="sunk"/> whether or not the <paramref name="shot"/>
        /// destroyed the <see cref="Ship"/>.
        /// </summary>
        /// <param name="shot">The <see cref="Shot"/> made earlier.</param>
        /// <param name="sunk">Indicates whether or not the <paramref name="shot"/> destroyed a ship.</param>
        void ShotHit(Shot shot, bool sunk);

        /// <summary>
        /// Called when a <paramref name="shot"/> created earlier did not hit an opposing <see cref="Controller"/>'s
        /// <see cref="Ship"/>.
        /// </summary>
        /// <param name="shot">The <see cref="Shot"/> created earlier.</param>
        void ShotMiss(Shot shot);
    }
}