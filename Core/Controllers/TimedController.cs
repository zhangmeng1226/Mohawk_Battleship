using MBC.Core.Threading;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MBC.Core.Controllers
{
    /// <summary>
    /// Wraps another IController and provides timeout features.
    /// </summary>
    [Configuration("mbc_player_timeout", 500)]
    public class TimedController : IController
    {
        private ThreadTimeoutAborter aborter;
        private IController controller;

        /// <summary>
        /// Wraps a controller and sets up the thread aborter on the current thread.
        /// </summary>
        /// <param name="controllerWrap"></param>
        public TimedController(MatchConfig config, IController controllerWrap)
        {
            controller = controllerWrap;
            Match = config;
            controller = controllerWrap;
            controller.ControllerMessageEvent += ReceiveMessage;
            aborter = new ThreadTimeoutAborter(Thread.CurrentThread, Match.TimeLimit);
        }

        /// <summary>
        /// Occurs whenever the <see cref="Controller"/> outputs a message string.
        /// </summary>
        public event StringOutputHandler ControllerMessageEvent;

        /// <summary>
        /// Gets or sets the wrapped field value for the controller.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the wrapped ID value for the controller.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the wrapped Match value for the controller.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the wrapped Registers value for the controller.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the wrapped Teams value for the controller.
        /// </summary>
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

        /// <summary>
        /// Gets a set attribute on a controller.
        /// </summary>
        /// <typeparam name="T">The type of attribute to get</typeparam>
        /// <returns>The attibute object of type <typeparamref name="T"/></returns>
        public T GetAttribute<T>()
        {
            return controller.GetAttribute<T>();
        }

        /// <summary>
        /// <see cref="IController.MakeShot()"/>
        /// </summary>
        /// <returns><see cref="IController.MakeShot()"/></returns>
        public Shot MakeShot()
        {
            try
            {
                aborter.MonBegin();
                var result = controller.MakeShot();
                aborter.MonEnd();
                return result;
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
                throw new ControllerTimeoutException("MakeShot", this);
            }
        }

        /// <summary>
        /// <see cref="IController.MatchOver()"/>
        /// </summary>
        public void MatchOver()
        {
            try
            {
                aborter.MonBegin();
                controller.MatchOver();
                aborter.MonEnd();
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
                throw new ControllerTimeoutException("MatchOver", this);
            }
        }

        /// <summary>
        /// <see cref="IController.NewMatch()"/>
        /// </summary>
        public void NewMatch()
        {
            try
            {
                aborter.MonBegin();
                controller.NewMatch();
                aborter.MonEnd();
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
                throw new ControllerTimeoutException("NewMatch", this);
            }
        }

        /// <summary>
        /// <see cref="IController.NewRound()"/>
        /// </summary>
        public void NewRound()
        {
            try
            {
                aborter.MonBegin();
                controller.NewRound();
                aborter.MonEnd();
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
                throw new ControllerTimeoutException("NewRound", this);
            }
        }

        /// <summary>
        /// <see cref="IController.OpponentDestroyed()"/>
        /// </summary>
        public void OpponentDestroyed(IDNumber destroyedID)
        {
            try
            {
                aborter.MonBegin();
                controller.OpponentDestroyed(destroyedID);
                aborter.MonEnd();
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
                throw new ControllerTimeoutException("OpponentDestroyed", this);
            }
        }

        /// <summary>
        /// <see cref="IController.OpponentShot()"/>
        /// </summary>
        public void OpponentShot(Shot shot)
        {
            try
            {
                aborter.MonBegin();
                controller.OpponentShot(shot);
                aborter.MonEnd();
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
                throw new ControllerTimeoutException("OpponentShot", this);
            }
        }

        /// <summary>
        /// <see cref="IController.PlaceShips()"/>
        /// </summary>
        /// <returns><see cref="IController.PlaceShips()"/></returns>
        public ShipList PlaceShips()
        {
            try
            {
                aborter.MonBegin();
                var result = controller.PlaceShips();
                aborter.MonEnd();
                return result;
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
                throw new ControllerTimeoutException("PlaceShips", this);
            }
        }

        /// <summary>
        /// <see cref="IController.RoundLost()"/>
        /// </summary>
        public void RoundLost()
        {
            try
            {
                aborter.MonBegin();
                controller.RoundLost();
                aborter.MonEnd();
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
                throw new ControllerTimeoutException("RoundLost", this);
            }
        }

        /// <summary>
        /// <see cref="IController.RoundWon()"/>
        /// </summary>
        public void RoundWon()
        {
            try
            {
                aborter.MonBegin();
                controller.RoundWon();
                aborter.MonEnd();
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
                throw new ControllerTimeoutException("RoundWon", this);
            }
        }

        /// <summary>
        /// <see cref="IController.ShotHit()"/>
        /// </summary>
        public void ShotHit(Shot shot, bool sunk)
        {
            try
            {
                aborter.MonBegin();
                controller.ShotHit(shot, sunk);
                aborter.MonEnd();
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
                throw new ControllerTimeoutException("ShotHit", this);
            }
        }

        /// <summary>
        /// <see cref="IController.ShotMiss()"/>
        /// </summary>
        public void ShotMiss(Shot shot)
        {
            try
            {
                aborter.MonBegin();
                controller.ShotMiss(shot);
                aborter.MonEnd();
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
                throw new ControllerTimeoutException("ShotMiss", this);
            }
        }

        /// <summary>
        /// <see cref="IController.ToString()"/>
        /// </summary>
        /// <returns><see cref="IController.ToString()"/></returns>
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