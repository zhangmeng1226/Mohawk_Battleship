using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MBC.Core.Util;
using MBC.Core.Threading;
using MBC.Core.Events;

namespace MBC.Core
{
    [Configuration("mbc_player_timeout", 500)]
    public class ControlledPlayer : IController
    {
        private Controller controller;

        private ControllerSkeleton controllerInfo;

        private TimedThreader threader;

        public Register Register
        {
            get
            {
                return controller.Register;
            }
            set
            {
                controller.Register = new Register(value);
            }
        }

        public ControllerSkeleton ControllerInfo
        {
            get
            {
                return controllerInfo;
            }
        }

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

        public Team Team 
        {
            get
            {
                return controller.Team;
            }
            set
            {
                controller.Team = new Team(value);
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
                controller.Registers = new Dictionary<IDNumber, Register>(value);
            }
        }

        public List<Team> Teams
        {
            get
            {
                return controller.Teams;
            }
            set
            {
                controller.Teams = new List<Team>(value);
            }
        }

        public ControlledPlayer(ControllerSkeleton targetController)
        {
            controllerInfo = targetController;

            controller = (Controller)Activator.CreateInstance(targetController.Controller);
            controller.ControllerMessageEvent += ReceiveMessage;

            threader = new TimedThreader();
        }

        /// <summary>
        /// Occurs whenever the <see cref="Controller"/> outputs a message string.
        /// </summary>
        public event StringOutputHandler ControllerMessageEvent;

        public Shot MakeShot()
        {
            threader.TimeMethod(new Func<Shot>(controller.MakeShot));
            return threader.GetReturnValue<Shot>();
        }

        public ShipList PlaceShips()
        {
            threader.TimeMethod(new Func<ShipList>(controller.PlaceShips));
            return threader.GetReturnValue<ShipList>();
        }

        /// <summary>
        /// <see cref="IController.NewMatch()"/>
        /// </summary>
        public void NewMatch()
        {
            threader.TimeMethod(new Action(controller.NewMatch));
        }

        /// <summary>
        /// <see cref="IController.MatchOver()"/>
        /// </summary>
        public void MatchOver()
        {
            threader.TimeMethod(new Action(controller.MatchOver));
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
            return controllerInfo.ToString();
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
