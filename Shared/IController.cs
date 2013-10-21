using System.Collections.Generic;

namespace MBC.Shared
{
    /// <summary>
    /// Defines a method that receives a string.
    /// </summary>
    /// <param name="message">A string containing the message being passed.</param>
    public delegate void StringOutputHandler(string message);

    public interface IController
    {
        /// <summary>
        /// Occurs whenever a string message is generated.
        /// </summary>
        event StringOutputHandler ControllerMessageEvent;

        FieldInfo Field { get; set; }

        IDNumber ID { get; set; }

        MatchConfig Match { get; set; }

        Dictionary<IDNumber, Register> Registers { get; set; }

        Dictionary<IDNumber, Team> Teams { get; set; }

        T GetAttribute<T>();

        /// <summary>
        /// Called when required to create and return a <see cref="Shot"/>. Refer to the rules of the
        /// <see cref="MatchConfig.GameMode"/> in the <see cref="Controller.Register"/> when creating the
        /// <see cref="Shot"/>.
        /// </summary>
        /// <returns>A <see cref="Shot"/> to be processed by the MBC core framework.</returns>
        Shot MakeShot();

        /// <summary>
        /// Called when the match against other <see cref="Controller"/>s is over.
        /// </summary>
        void MatchOver();

        /// <summary>
        /// Called when entered in a new match. The <see cref="Controller.Register"/> will have
        /// been updated with new information.
        /// </summary>
        void NewMatch();

        /// <summary>
        /// Called when entered in a new round. The <see cref="Controller.Register"/> <see cref="Register.Ships"/>
        /// will be reset to an unplaced state.
        /// </summary>
        void NewRound();

        /// <summary>
        /// Called when an opposing <see cref="Controller"/> has had all of their <see cref="Ship"/>s destroyed,
        /// lost the round, and has been removed from the active <see cref="Controller"/>s in the round.
        /// </summary>
        /// <param name="destroyedID">The <see cref="IDNumber"/> of the <see cref="Controller"/> that
        /// is no longer an opponent.</param>
        void OpponentDestroyed(IDNumber destroyedID);

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
        ShipList PlaceShips();

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