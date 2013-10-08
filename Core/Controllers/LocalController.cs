using System;
using System.Collections.Generic;
using MBC.Core.Threading;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;

namespace MBC.Core.Controllers
{
    [Configuration("mbc_player_timeout", 500)]
    public class LocalController : IController
    {
        private IController controller;

        private ControllerSkeleton skeleton;
        private TimedThreader threader;

        public LocalController(ControllerSkeleton targetController)
        {
            skeleton = targetController;

            controller = (Controller)Activator.CreateInstance(targetController.Controller);
            controller.ControllerMessageEvent += ReceiveMessage;

            threader = new TimedThreader();
        }

        /// <summary>
        /// Occurs whenever the <see cref="Controller"/> outputs a message string.
        /// </summary>
        public event StringOutputHandler ControllerMessageEvent;

        public FieldInfo Field
        {
            get
            {
                return controller.Field;
            }
            set
            {
                controller.Field = new FieldInfo(value);
            }
        }

        public IDNumber ID
        {
            get
            {
                return controller.ID;
            }
            set
            {
                controller.ID = value;
            }
        }

        public MatchConfig Match
        {
            get
            {
                return controller.Match;
            }
            set
            {
                controller.Match = new MatchConfig(value);
            }
        }

        public Dictionary<IDNumber, Register> Registers
        {
            get
            {
                return controller.Registers;
            }
            set
            {
                controller.Registers = new Dictionary<IDNumber, Register>();
                foreach (var register in value)
                {
                    controller.Registers.Add(register.Key, new Register(register.Value));
                }
            }
        }

        public ControllerSkeleton Skeleton
        {
            get
            {
                return skeleton;
            }
        }

        public Dictionary<IDNumber, Team> Teams
        {
            get
            {
                return controller.Teams;
            }
            set
            {
                controller.Teams = new Dictionary<IDNumber, Team>();
                foreach (var team in value)
                {
                    controller.Teams.Add(team.Key, new Team(team.Value));
                }
            }
        }

        public T GetAttribute<T>()
        {
            return skeleton.GetAttribute<T>();
        }

        public Shot MakeShot()
        {
            threader.TimeMethod(new Func<Shot>(controller.MakeShot));
            return threader.GetReturnValue<Shot>();
        }

        /// <summary>
        /// <see cref="IController.MatchOver()"/>
        /// </summary>
        public void MatchOver()
        {
            threader.TimeMethod(new Action(controller.MatchOver));
        }

        /// <summary>
        /// <see cref="IController.NewMatch()"/>
        /// </summary>
        public void NewMatch()
        {
            threader.TimeMethod(new Action(controller.NewMatch));
        }

        /// <summary>
        /// <see cref="IController.NewRound()"/>
        /// </summary>
        public void NewRound()
        {
            threader.TimeMethod(new Action(controller.NewRound));
        }

        /// <summary>
        /// <see cref="IController.OpponentDestroyed()"/>
        /// </summary>
        public void OpponentDestroyed(IDNumber destroyedID)
        {
            threader.TimeMethod(new Action<IDNumber>(controller.OpponentDestroyed), destroyedID);
        }

        /// <summary>
        /// <see cref="IController.OpponentShot()"/>
        /// </summary>
        public void OpponentShot(Shot shot)
        {
            threader.TimeMethod(new Action<Shot>(controller.OpponentShot), new Shot(shot));
        }

        public ShipList PlaceShips()
        {
            threader.TimeMethod(new Func<ShipList>(controller.PlaceShips));
            return threader.GetReturnValue<ShipList>();
        }

        /// <summary>
        /// <see cref="IController.RoundLost()"/>
        /// </summary>
        public void RoundLost()
        {
            threader.TimeMethod(new Action(controller.RoundLost));
        }

        /// <summary>
        /// <see cref="IController.RoundWon()"/>
        /// </summary>
        public void RoundWon()
        {
            threader.TimeMethod(new Action(controller.RoundWon));
        }

        /// <summary>
        /// <see cref="IController.ShotHit()"/>
        /// </summary>
        public void ShotHit(Shot shot, bool sunk)
        {
            threader.TimeMethod(new Action<Shot, bool>(controller.ShotHit), shot, sunk);
        }

        /// <summary>
        /// <see cref="IController.ShotMiss()"/>
        /// </summary>
        public void ShotMiss(Shot shot)
        {
            threader.TimeMethod(new Action<Shot>(controller.ShotMiss));
        }

        public override string ToString()
        {
            return skeleton.ToString();
        }

        /// <summary>
        /// This method is subscribed to the <see cref="Controller"/>'s <see cref="Controller.ControllerMessageEvent"/>.
        /// </summary>
        /// <param name="message">A string containing the message.</param>
        private void ReceiveMessage(string message)
        {
            if (ControllerMessageEvent != null)
            {
                ControllerMessageEvent(message);
            }
        }
    }
}