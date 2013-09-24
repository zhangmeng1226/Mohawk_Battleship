using MBC.Core.Rounds;
using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// The base class for a series of <see cref="Event"/>s that provide information on the actions
    /// made in rounds from controllers. Provides the <see cref="IDNumber"/> of the <see cref="Controller"/>
    /// that caused the event to be created.
    /// </summary>
    public abstract class PlayerEvent : Event
    {
        /// <summary>
        /// Copies the <see cref="Register"/> object reference.
        /// </summary>
        /// <param name="register"></param>
        public PlayerEvent(Player plr)
        {
            Player = plr;
        }

        private PlayerEvent() { }

        /// <summary>
        /// Gets the <see cref="Register"/> that caused this event to occur.
        /// </summary>
        public Player Player
        {
            get;
            private set;
        }
    }
}