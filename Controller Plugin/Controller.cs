using System;
using System.Collections.Generic;
using System.Linq;
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

        protected List<IDNumber> AllOpponents()
        {
            var opponents = new HashSet<IDNumber>();
            foreach (var team in Teams)
            {
                if (!team.Value.IsInternal && (!team.Value.Members.Contains(ID) || !team.Value.IsFriendly))
                {
                    foreach (var id in team.Value.Members)
                    {
                        if (id != ID)
                        {
                            opponents.Add(id);
                        }
                    }
                }
            }
            return opponents.ToList();
        }

        protected Shot CreateShot(Coordinates shotCoords)
        {
            return CreateShot(NextOpponent(), shotCoords);
        }

        protected Shot CreateShot(int xCoord, int yCoord)
        {
            return CreateShot(NextOpponent(), new Coordinates(xCoord, yCoord));
        }

        protected Shot CreateShot(IDNumber opponent, Coordinates shotCoords)
        {
            return new Shot(opponent, shotCoords);
        }

        protected IDNumber NextOpponent()
        {
            foreach (var team in Teams)
            {
                if (team.Value.Members.Count > 0 && !team.Value.Members.Contains(ID))
                {
                    return team.Value.Members.First();
                }
            }
            return -1;
        }

        /// <summary>
        /// Used to output a <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The string to output to the MBC core framework.</param>
        protected void SendMessage(string message)
        {
            if (ControllerMessageEvent != null)
            {
                ControllerMessageEvent(message);
            }
        }
    }
}