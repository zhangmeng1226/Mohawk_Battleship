using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;

namespace MBC.Core
{

    /// <summary>
    /// The Controller class also represents a loaded controller implementing the IBattleshipController class
    /// and wraps this class to provide various utilities such as timing.
    /// </summary>
    [Configuration("mbc_controller_thread_timeout", 500)]
    public class ControllerUser
    {
        private Controller controller;

        private ControllerInformation controllerInfo;

        private ControllerRegister register;

        private int maxTimeout;

        private Stopwatch timeElapsed;

        /// <summary>
        /// Constructs a new Controller object with the given ClassInfo controller information and MatchInfo.
        /// </summary>
        /// <param name="targetControllerInfo">The ClassInfo to create the object from.</param>
        /// <param name="matchInfo">The MatchInfo to set the behaviour of this Controller for.</param>
        public ControllerUser(ControllerInformation targetControllerInfo, ControllerRegister register)
        {
            this.controllerInfo = targetControllerInfo;
            this.timeElapsed = new Stopwatch();
            this.maxTimeout = Configuration.Global.GetValue<int>("mbc_controller_thread_timeout");

            this.register = register;

            controller = (Controller)Activator.CreateInstance(targetControllerInfo.Controller);
            controller.ControllerMessageEvent += ReceiveMessage;
        }

        /// <summary>
        /// Invoked whenever the underlying controller interface wants to output a message string.
        /// </summary>
        public event StringOutputHandler ControllerMessageEvent;

        /// <summary>
        /// Gets the ClassInfo that this Controller object refers to.
        /// </summary>
        /// <seealso cref="ClassInfo"/>
        public ControllerInformation Information
        {
            get
            {
                return controllerInfo;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this Controller wraps around a controller interface.
        /// </summary>
        public bool InterfaceExists
        {
            get
            {
                return controller != null;
            }
        }

        /// <summary>
        /// Gets the time (in milliseconds) that the controller interface took to finish the last method invoked.
        /// </summary>
        public int TimeElapsed
        {
            get
            {
                return (int)timeElapsed.ElapsedMilliseconds;
            }
        }

        /// <summary>
        /// Gets the name of the controller interface.
        /// </summary>
        public string Name
        {
            get
            {
                return controllerInfo.Name;
            }
        }

        /// <summary>
        /// Gets the Version number of the controller interface.
        /// </summary>
        public Version Version
        {
            get
            {
                return controllerInfo.Version;
            }
        }

        /// <summary>
        /// Gets a string representation of this Controller that may be used for display.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return Name+" v("+Version.ToString()+")";
            }
        }

        public ControllerRegister Register
        {
            get
            {
                return register;
            }
        }

        /// <summary>
        /// Determines whether or not this Controller had exceeded the allowed time limit during the last method
        /// invoke.
        /// </summary>
        /// <returns>true if this Controller passed the time limit, false otherwise.</returns>
        public bool TimedOut()
        {
            return TimeElapsed > Register.Match.TimeLimit;
        }

        /// <summary>
        /// Resets match-related information made by this Controller and invokes the NewMatch method on the
        /// controller interface. Copies the information given instead of passing them to the controller interface
        /// by reference.
        /// </summary>
        /// <param name="opponentName">A string representing the name of the newly matched up opponent controller.</param>
        /// <param name="fieldSize">The size of the battlefield.</param>
        /// <param name="methodTime">The time limit given for this Controller.</param>
        /// <param name="initShips">The ships that a match will play with.</param>
        public void NewMatch()
        {
            Register.Score = 0;
            controller.Register = new ControllerRegister(Register);

            var thread = new Thread(() =>
            controller.NewMatch());

            HandleThread(thread, "NewMatch");
        }

        /// <summary>
        /// Resets round-related information made by this Controller and invokes the NewRound method on the
        /// controller interface.
        /// </summary>
        public void NewRound()
        {
            Register.Shots = new ShotList();
            Register.Ships = new ShipList(Register.Match.StartingShips);

            controller.Register = new ControllerRegister(Register);

            var thread = new Thread(() => controller.NewRound());

            HandleThread(thread, "NewRound");
        }

        /// <summary>
        /// Copies the initial ShipList from the match info and passes this list to the
        /// method PlaceShips in the controller interface to be invoked.
        /// </summary>
        public void PlaceShips()
        {
            var thread = new Thread(() => controller.PlaceShips());

            HandleThread(thread, "PlaceShips");
            Register.Ships = new ShipList(controller.Register.Ships);
        }

        /// <summary>
        /// Invokes the controller interface's GetShot method to get the Coordinates of its next shot.
        /// </summary>
        /// <returns>A Coordinates object of the controller's shot. If null, the controller interface failed to
        /// return the Coordinates within the time limit.</returns>
        public Shot MakeShot()
        {
            Shot result = null;
            var thread = new Thread(() => result = controller.MakeShot());

            HandleThread(thread, "GetShot");

            return result;
        }

        /// <summary>
        /// Notifies this controller interface about a shot made by the opposing controller. Invokes the
        /// OpponentShot method with the Coordinates of the shot made.
        /// </summary>
        /// <param name="opShot">The Coordinates of the opposing controller's shot.</param>
        public void NotifyOpponentShot(Shot opShot)
        {
            var thread = new Thread(() => controller.OpponentShot(opShot));

            HandleThread(thread, "OpponentShot");
        }

        /// <summary>
        /// Notifies this controller about a shot that was previously made that had shot an opposing controller's
        /// Ship with an indication of whether or not the shot sunk the ship. Invokes the ShotHit method
        /// of the controller interface with this information.
        /// </summary>
        /// <param name="shotMade">The Coordinates of the shot previously made.</param>
        /// <param name="sink">true if the Coordinates of the shot sunk a ship, false otherwise.</param>
        public void NotifyShotHit(Shot shotMade, bool sink)
        {
            var thread = new Thread(() => controller.ShotHit(shotMade, sink));

            HandleThread(thread, "ShotHit");
        }

        /// <summary>
        /// Notifies this controller about a shot that has been previously made that had missed an opposing
        /// controller's Ship. Invokes the controller interface's ShotMiss method with the Coordinates of this
        /// Shot.
        /// </summary>
        /// <param name="shotMade">The Coordinates of the shot that missed.</param>
        public void NotifyShotMiss(Shot shotMade)
        {
            var thread = new Thread(() => controller.ShotMiss(shotMade));

            HandleThread(thread, "ShotMiss");
        }

        /// <summary>
        /// Adds to this Controller object's score and invokes the RoundWon method on the controller interface.
        /// </summary>
        public void RoundWon()
        {
            Register.Score++;
            controller.Register.Score++;

            var thread = new Thread(() =>
            controller.RoundWon());

            HandleThread(thread, "RoundWon");
        }

        /// <summary>
        /// Invokes the RoundLost method on the controller interface.
        /// </summary>
        public void RoundLost()
        {
            var thread = new Thread(() =>
            controller.RoundLost());

            HandleThread(thread, "RoundLost");
        }

        /// <summary>
        /// Resets Match-specific variables to null values and invokes the controller interface's MatchOver
        /// method.
        /// </summary>
        public void MatchOver()
        {
            var thread = new Thread(() =>
            controller.MatchOver());

            HandleThread(thread, "MatchOver");
        }

        /// <summary>
        /// Gets a string representation of this Controller. This is the same as the DisplayName property.
        /// </summary>
        /// <returns>A string that names this Controller.</returns>
        public override string ToString()
        {
            return DisplayName;
        }

        /// <summary>
        /// Invoked when the controller interface wants to output a string.
        /// </summary>
        /// <param name="message">A string containing the message.</param>
        private void ReceiveMessage(string message)
        {
            if (ControllerMessageEvent != null)
            {
                ControllerMessageEvent(message);
            }
        }

        /// <summary>
        /// Handles a given Thread by providing a timing and timeout function. If there is no controller interface
        /// loaded with this controller, the thread will not be called.
        /// </summary>
        /// <param name="thread">The Thread to monitor and time.</param>
        private void HandleThread(Thread thread, string method)
        {
            //Start the thread.
            timeElapsed.Restart();
            thread.Start();
            if (!thread.Join(maxTimeout))
            {
                //Thread timed out.
                thread.Abort();
            }
            timeElapsed.Stop();
            if (TimedOut())
            {
                throw new ControllerTimeoutException(Register, method, TimeElapsed);
            }
        }
    }
}
