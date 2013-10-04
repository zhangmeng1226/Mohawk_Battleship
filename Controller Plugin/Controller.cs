using System.Collections.Generic;
using System.Security.Permissions;

namespace MBC.Shared
{
    /// <summary>
    /// <para>
    /// Provides a number of overrideable methods that are invoked during a match, and is provided with
    /// a copy of a <see cref="Register"/> for use.
    /// </para>
    /// <para>
    /// At a minimum, certain attributes must be set to a deriving class, which are the <see cref="Attributes.NameAttribute"/>,
    /// <see cref="Attributes.VersionAttribute"/>, and <see cref="Attributes.CapabilitiesAttribute"/>. These
    /// are required to be defined in order for a deriving class to be loaded into the MBC core framework.
    /// </para>
    /// </summary>
    [SecurityPermission(SecurityAction.PermitOnly, SerializationFormatter = true)]
    public abstract class Controller : IController
    {
        /// <summary>
        /// <see cref="IController.ControllerMessageEvent"/>
        /// </summary>
        public event StringOutputHandler ControllerMessageEvent;

        /// <summary>
        /// Gets or sets the <see cref="Register"/>.
        /// </summary>
        public Register Register { get; set; }

        public FieldInfo Field { get; set; }

        public MatchConfig Match { get; set; }

        public Team Team { get; set; }

        public Dictionary<IDNumber, Register> Registers
        {
            get;
            set;
        }

        public List<Team> Teams
        {
            get;
            set;
        }

        public IDNumber NextOpponent()
        {
            return 0;
        }

        public List<IDNumber> AllOpponents()
        {
            List<IDNumber> opponents = new List<IDNumber>();
            foreach (var team in Teams)
            {
                foreach (var member in team.Members)
                {
                    opponents.Add(member);
                }
            }
            return opponents;
        }

        /// <summary>
        /// <see cref="IController.NewMatch()"/>
        /// </summary>
        public virtual void NewMatch()
        {
        }

        public abstract Shot MakeShot();

        public abstract ShipList PlaceShips();

        /// <summary>
        /// <see cref="IController.MatchOver()"/>
        /// </summary>
        public virtual void MatchOver()
        {
        }

        /// <summary>
        /// <see cref="IController.NewRound()"/>
        /// </summary>
        public virtual void NewRound()
        {
        }

        /// <summary>
        /// <see cref="IController.OpponentDestroyed()"/>
        /// </summary>
        public virtual void OpponentDestroyed(IDNumber destroyedID)
        {
        }

        /// <summary>
        /// <see cref="IController.OpponentShot()"/>
        /// </summary>
        public virtual void OpponentShot(Shot shot)
        {
        }

        /// <summary>
        /// <see cref="IController.RoundLost()"/>
        /// </summary>
        public virtual void RoundLost()
        {
        }

        /// <summary>
        /// <see cref="IController.RoundWon()"/>
        /// </summary>
        public virtual void RoundWon()
        {
        }

        /// <summary>
        /// <see cref="IController.ShotHit()"/>
        /// </summary>
        public virtual void ShotHit(Shot shot, bool sunk)
        {
        }

        /// <summary>
        /// <see cref="IController.ShotMiss()"/>
        /// </summary>
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