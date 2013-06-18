using System.Security.Permissions;

namespace MBC.Shared
{
    /// <summary>
    /// Defines a method that receives a string.
    /// </summary>
    /// <param name="message">A string containing the message being passed.</param>
    public delegate void StringOutputHandler(string message);

    /// <summary>
    /// <para>
    /// Provides a number of overrideable methods that are invoked during a match, and is provided with
    /// a copy of a <see cref="ControllerRegister"/> for use.
    /// </para>
    /// <para>
    /// At a minimum, certain attributes must be set to a deriving class, which are the <see cref="Attributes.NameAttribute"/>,
    /// <see cref="Attributes.VersionAttribute"/>, and <see cref="Attributes.CapabilitiesAttribute"/>. These
    /// are required to be defined in order for a deriving class to be loaded into the MBC core framework.
    /// </para>
    /// </summary>
    [SecurityPermission(SecurityAction.PermitOnly, SerializationFormatter = true)]
    public abstract class Controller
    {
        /// <summary>
        /// Occurs whenever a string message is generated.
        /// </summary>
        public event StringOutputHandler ControllerMessageEvent;

        /// <summary>
        /// Gets or sets the <see cref="ControllerRegister"/> that is available for manipulation.
        /// </summary>
        public ControllerRegister Register { get; set; }

        /// <summary>
        /// Called when required to create and return a <see cref="Shot"/>. Refer to the rules of the
        /// <see cref="MatchInfo.GameMode"/> in the <see cref="Controller.Register"/> when creating the
        /// <see cref="Shot"/>.
        /// </summary>
        /// <returns>A <see cref="Shot"/> to be processed by the MBC core framework.</returns>
        public abstract Shot MakeShot();

        /// <summary>
        /// Called when the match against other <see cref="Controller"/>s is over.
        /// </summary>
        public virtual void MatchOver()
        {
        }

        /// <summary>
        /// Called when entered in a new match. The <see cref="Controller.Register"/> will have
        /// been updated with new information.
        /// </summary>
        public virtual void NewMatch()
        {
        }

        /// <summary>
        /// Called when entered in a new round. The <see cref="Controller.Register"/> <see cref="ControllerRegister.Ships"/>
        /// will be reset to an unplaced state.
        /// </summary>
        public virtual void NewRound()
        {
        }

        /// <summary>
        /// Called when an opposing <see cref="Controller"/> has had all of their <see cref="Ship"/>s destroyed,
        /// lost the round, and has been removed from the active <see cref="Controller"/>s in the round.
        /// </summary>
        /// <param name="destroyedID">The <see cref="ControllerID"/> of the <see cref="Controller"/> that
        /// is no longer an opponent.</param>
        public virtual void OpponentDestroyed(ControllerID destroyedID)
        {
        }

        /// <summary>
        /// Called when an opposing <see cref="Controller"/> in the same match has created a
        /// <see cref="Shot"/> against this <see cref="Controller"/>.
        /// </summary>
        /// <param name="shot">The <see cref="Shot"/> made by an opposing <see cref="Controller"/></param>
        public virtual void OpponentShot(Shot shot)
        {
        }

        /// <summary>
        /// Called when the <see cref="ControllerRegister.Ships"/> in the <see cref="Controller.Register"/> must
        /// be placed. Refer to the rules of the <see cref="MatchInfo.GameMode"/> in the <see cref="Controller.Register"/>.
        /// </summary>
        public abstract void PlaceShips();

        /// <summary>
        /// Called when the round has been lost.
        /// </summary>
        public virtual void RoundLost()
        {
        }

        /// <summary>
        /// Called when the round has been won.
        /// </summary>
        public virtual void RoundWon()
        {
        }

        /// <summary>
        /// Called when a <paramref name="shot"/> created earlier hit an opposing <see cref="Controller"/>'s
        /// <see cref="Ship"/>. Indicates via <paramref name="sunk"/> whether or not the <paramref name="shot"/>
        /// destroyed the <see cref="Ship"/>.
        /// </summary>
        /// <param name="shot">The <see cref="Shot"/> made earlier.</param>
        /// <param name="sunk">Indicates whether or not the <paramref name="shot"/> destroyed a ship.</param>
        public virtual void ShotHit(Shot shot, bool sunk)
        {
        }

        /// <summary>
        /// Called when a <paramref name="shot"/> created earlier did not hit an opposing <see cref="Controller"/>'s
        /// <see cref="Ship"/>.
        /// </summary>
        /// <param name="shot">The <see cref="Shot"/> created earlier.</param>
        public virtual void ShotMiss(Shot shot)
        {
        }

        /// <summary>
        /// Used to output a <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The string to output to the MBC core framework.</param>
        protected void SendMessage(string message)
        {
            ControllerMessageEvent(message);
        }
    }
}