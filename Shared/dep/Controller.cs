using MBC.Shared;
using MBC.Shared.Entities;
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
    [Obsolete("Use and implement the IController2 interface.")]
    public abstract class Controller : IController
    {
        /// <summary>
        /// <see cref="IController.ControllerMessageEvent"/>
        /// </summary>
        public event StringOutputHandler ControllerMessageEvent;

        /// <summary>
        /// The field that represents the current match.
        /// </summary>
        public FieldInfo Field { get; set; }

        /// <summary>
        /// Gets the identification number assigned to this controller through the match.
        /// </summary>
        public IDNumber ID { get; set; }

        /// <summary>
        /// Gets the match parameters set in the match.
        /// </summary>
        public MatchConfig Match { get; set; }

        /// <summary>
        /// Gets the register object associated with this controller.
        /// </summary>
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

        /// <summary>
        /// Gets the team this controller is a member of.
        /// </summary>
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

        /// <summary>
        /// Gets the ControllerRegister object associated with this controller. You
        /// should not use this.
        /// </summary>
        [Obsolete("Use alternative fields, properties, and methods provided in this class.")]
        public ControllerRegister Register
        {
            get
            {
                return new ControllerRegister(this);
            }
        }

        /// <summary>
        /// Gets the registers in the match.
        /// </summary>
        public Dictionary<IDNumber, Register> Registers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the teams created within the match.
        /// </summary>
        public Dictionary<IDNumber, Team> Teams
        {
            get;
            set;
        }

        /// <summary>
        /// Gets all of the opponents of this controller (not part of the same team)
        /// </summary>
        /// <returns>Identification numbers of all of the opponent controllers.</returns>
        public List<IDNumber> AllOpponents()
        {
            var opponents = new HashSet<IDNumber>();
            foreach (var team in Teams)
            {
                if (team.Value != MyTeam)
                {
                    foreach (IDNumber plr in team.Value.Members)
                    {
                        if (plr != ID)
                        {
                            opponents.Add(plr);
                        }
                    }
                }
            }
            return opponents.ToList();
        }

        /// <summary>
        /// Gets an attribute that has been set upon a controller.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetAttribute<T>()
        {
            object[] attribute = GetType().GetCustomAttributes(typeof(T), false);
            if (attribute != null && attribute.Length > 0)
            {
                return (T)attribute[0];
            }
            return default(T);
        }

        /// <summary>
        /// Called when
        /// </summary>
        /// <returns></returns>
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

        public virtual ShipList PlaceShips()
        {
            return PlaceShips(Match.StartingShips);
        }

        [Obsolete("initialShips no longer needs to be provided as a parameter. See MatchConfig.StartingShips instead.")]
        public virtual ShipList PlaceShips(ShipList initialShips)
        {
            return null;
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
            return AllOpponents()[0];
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