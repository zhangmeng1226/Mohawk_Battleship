using System;
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

        public FieldInfo Field { get; set; }

        public IDNumber ID { get; set; }

        public MatchConfig Match { get; set; }

        public Register MyRegister
        {
            get
            {
                foreach (var register in Registers)
                {
                    if (register.Value.ID == ID)
                    {
                        return register.Value;
                    }
                }
                return null;
            }
        }

        public Team MyTeam
        {
            get
            {
                foreach (var team in Teams)
                {
                    if (team.Value.Members.Contains(ID))
                    {
                        return team.Value;
                    }
                }
                return null;
            }
        }

        public Dictionary<IDNumber, Register> Registers
        {
            get;
            set;
        }

        public Dictionary<IDNumber, Team> Teams
        {
            get;
            set;
        }

        public List<IDNumber> AllOpponents()
        {
            throw new NotImplementedException();
        }

        public T GetAttribute<T>()
        {
            object[] attribute = GetType().GetCustomAttributes(typeof(T), false);
            if (attribute != null && attribute.Length > 0)
            {
                return (T)attribute[0];
            }
            return default(T);
        }

        public abstract Shot MakeShot();

        /// <summary>
        /// <see cref="IController.MatchOver()"/>
        /// </summary>
        public virtual void MatchOver()
        {
        }

        /// <summary>
        /// <see cref="IController.NewMatch()"/>
        /// </summary>
        public virtual void NewMatch()
        {
        }

        /// <summary>
        /// <see cref="IController.NewRound()"/>
        /// </summary>
        public virtual void NewRound()
        {
        }

        public IDNumber NextOpponent()
        {
            return 0;
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

        public abstract ShipList PlaceShips();

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