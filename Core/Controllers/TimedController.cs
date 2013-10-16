using System;
using System.Collections.Generic;
using System.Diagnostics;
using MBC.Core.Threading;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;

namespace MBC.Core.Controllers
{
    [Configuration("mbc_player_timeout", 500)]
    public class TimedController : IController
    {
        private IController controller;
        private Stopwatch timer;

        public TimedController(IController controllerWrap)
        {
            controller = controllerWrap;
            controller.ControllerMessageEvent += ReceiveMessage;
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
                controller.Field = value;
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
                controller.Match = value;
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
                controller.Registers = value;
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
                controller.Teams = value;
            }
        }

        public T GetAttribute<T>()
        {
            return controller.GetAttribute<T>();
        }

        public Shot MakeShot()
        {
            timer.Restart();
            var result = controller.MakeShot();
            timer.Stop();
            if (timer.ElapsedMilliseconds > Match.TimeLimit)
            {
                throw new MethodTimeoutException("MakeShot", (int)timer.ElapsedMilliseconds);
            }
            return result;
        }

        /// <summary>
        /// <see cref="IController.MatchOver()"/>
        /// </summary>
        public void MatchOver()
        {
            timer.Restart();
            controller.MatchOver();
            timer.Stop();
            if (timer.ElapsedMilliseconds > Match.TimeLimit)
            {
                throw new MethodTimeoutException("MatchOver", (int)timer.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// <see cref="IController.NewMatch()"/>
        /// </summary>
        public void NewMatch()
        {
            timer.Restart();
            controller.NewMatch();
            timer.Stop();
            if (timer.ElapsedMilliseconds > Match.TimeLimit)
            {
                throw new MethodTimeoutException("NewMatch", (int)timer.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// <see cref="IController.NewRound()"/>
        /// </summary>
        public void NewRound()
        {
            timer.Restart();
            controller.NewRound();
            timer.Stop();
            if (timer.ElapsedMilliseconds > Match.TimeLimit)
            {
                throw new MethodTimeoutException("NewRound", (int)timer.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// <see cref="IController.OpponentDestroyed()"/>
        /// </summary>
        public void OpponentDestroyed(IDNumber destroyedID)
        {
            timer.Restart();
            controller.OpponentDestroyed(destroyedID);
            timer.Stop();
            if (timer.ElapsedMilliseconds > Match.TimeLimit)
            {
                throw new MethodTimeoutException("OpponentDestroyed", (int)timer.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// <see cref="IController.OpponentShot()"/>
        /// </summary>
        public void OpponentShot(Shot shot)
        {
            timer.Restart();
            controller.OpponentShot(shot);
            timer.Stop();
            if (timer.ElapsedMilliseconds > Match.TimeLimit)
            {
                throw new MethodTimeoutException("OpponentShot", (int)timer.ElapsedMilliseconds);
            }
        }

        public ShipList PlaceShips()
        {
            timer.Restart();
            var result = controller.PlaceShips();
            timer.Stop();
            if (timer.ElapsedMilliseconds > Match.TimeLimit)
            {
                throw new MethodTimeoutException("PlaceShips", (int)timer.ElapsedMilliseconds);
            }
            return result;
        }

        /// <summary>
        /// <see cref="IController.RoundLost()"/>
        /// </summary>
        public void RoundLost()
        {
            timer.Restart();
            controller.RoundLost();
            timer.Stop();
            if (timer.ElapsedMilliseconds > Match.TimeLimit)
            {
                throw new MethodTimeoutException("RoundLost", (int)timer.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// <see cref="IController.RoundWon()"/>
        /// </summary>
        public void RoundWon()
        {
            timer.Restart();
            controller.RoundWon();
            timer.Stop();
            if (timer.ElapsedMilliseconds > Match.TimeLimit)
            {
                throw new MethodTimeoutException("RoundWon", (int)timer.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// <see cref="IController.ShotHit()"/>
        /// </summary>
        public void ShotHit(Shot shot, bool sunk)
        {
            timer.Restart();
            controller.ShotHit(shot, sunk);
            timer.Stop();
            if (timer.ElapsedMilliseconds > Match.TimeLimit)
            {
                throw new MethodTimeoutException("ShotHit", (int)timer.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// <see cref="IController.ShotMiss()"/>
        /// </summary>
        public void ShotMiss(Shot shot)
        {
            timer.Restart();
            controller.ShotMiss(shot);
            timer.Stop();
            if (timer.ElapsedMilliseconds > Match.TimeLimit)
            {
                throw new MethodTimeoutException("ShotMiss", (int)timer.ElapsedMilliseconds);
            }
        }

        public override string ToString()
        {
            return controller.ToString();
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