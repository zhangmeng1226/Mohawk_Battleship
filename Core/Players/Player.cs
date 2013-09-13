using MBC.Core.Util;
using MBC.Shared;
using System;
using System.Diagnostics;
using System.Threading;

namespace MBC.Core
{
    /// <summary>
    /// Loads a <see cref="Controller"/> from <see cref="ControllerInformation"/>. Wraps the <see cref="Controller"/>
    /// to invoke its methods in a different thread and prevent hang-ups due to
    /// <see cref="Controller"/>s taking too long to complete method calls.
    /// </summary>
    [Configuration("mbc_player_thread_timeout", 500)]
    public abstract class Player : ControllerRegister
    {

        protected Player()
        {

        }

        public ShipList Ships { get; internal set; }
        public ShotList Shots { get; internal set; }
        public ShipList ShipsLeft { get; internal set; }
        public ShotList ShotsAgainst { get; internal set; }
        public Shot LastShot { get; internal set; }

        /// <summary>
        /// Occurs whenever the <see cref="Controller"/> outputs a message string.
        /// </summary>
        public event StringOutputHandler ControllerMessageEvent;

        public virtual Shot MakeShot()
        {
            return null;
        }

        public virtual void MatchOver()
        {

        }

        public virtual void NewMatch(Register registerInstance)
        {
            Score = 0;
        }

        public virtual void NewRound()
        {

        }

        public virtual void NotifyOpponentShot(Shot opShot)
        {

        }

        public virtual void NotifyShotHit(Shot shotMade, bool sink)
        {

        }

        public virtual void NotifyShotMiss(Shot shotMade)
        {

        }

        public virtual ShipList PlaceShips()
        {
            return null;
        }

        public virtual void RoundLost()
        {

        }

        public virtual void RoundWon()
        {
            Score++;
        }

        /// <summary>
        /// Runs a <see cref="Thread"/> and waits for it to finish for a time as defined in the <see cref="Configuration"/>.
        /// Throws a <see cref="ControllerTimeoutException"/> if the time limit has been exceeded.
        /// </summary>
        /// <param name="thread">The thread to start.</param>
        /// <param name="method">The name of the method of the <see cref="Controller"/> being ran.
        /// Used as information for a <see cref="ControllerTimeoutException"/>.</param>
        /// <exception cref="ControllerTimeoutException">Thrown if the controller exceeded the time limit specified
        /// in the <see cref="MatchInfo"/> located in the <see cref="ControllerRegister"/>.</exception>
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
            if (TimeElapsed > Match.TimeLimit)
            {
                throw new ControllerTimeoutException(this, method, TimeElapsed);
            }
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

        private class DummyPlayer : Player
        {
            private string playerName;

            public DummyPlayer(string name)
            {
                playerName = name;
            }

            public override string ToString()
            {
                return playerName;
            }
        }

        private class ControllerPlayer : Player
        {
            private Controller controller;

            private ControllerInformation controllerInfo;

            public ControllerPlayer(ControllerInformation targetControllerInfo)
            {
                this.controllerInfo = targetControllerInfo;
                this.timeElapsed = new Stopwatch();
                this.maxTimeout = Configuration.Global.GetValue<int>("mbc_player_thread_timeout");

                controller = (Controller)Activator.CreateInstance(targetControllerInfo.Controller);
                controller.ControllerMessageEvent += ReceiveMessage;
            }

            public override string ToString()
            {
                return controllerInfo.ToString();
            }

            public Shot MakeShot()
            {
                Shot result = null;
                var thread = new Thread(() => result = controller.MakeShot());

                HandleThread(thread, "GetShot");

                return result;
            }

            public void MatchOver()
            {
                var thread = new Thread(() =>
                controller.MatchOver());

                HandleThread(thread, "MatchOver");
            }

            public void NewMatch(Register registerInstance)
            {
                Register = registerInstance;
                Register.Score = 0;
                controller.Register = new ControllerRegister(Register);

                var thread = new Thread(() =>
                controller.NewMatch());

                HandleThread(thread, "NewMatch");
            }

            public void NewRound()
            {
                var thread = new Thread(() => controller.NewRound());

                HandleThread(thread, "NewRound");
            }

            public void NotifyOpponentShot(Shot opShot)
            {
                var thread = new Thread(() => controller.OpponentShot(opShot));

                HandleThread(thread, "OpponentShot");
            }

            public void NotifyShotHit(Shot shotMade, bool sink)
            {
                var thread = new Thread(() => controller.ShotHit(shotMade, sink));

                HandleThread(thread, "ShotHit");
            }

            public void NotifyShotMiss(Shot shotMade)
            {
                var thread = new Thread(() => controller.ShotMiss(shotMade));

                HandleThread(thread, "ShotMiss");
            }

            public ShipList PlaceShips()
            {
                ShipList result = null;
                var thread = new Thread(() => result = controller.PlaceShips(Register.Match.StartingShips));

                HandleThread(thread, "PlaceShips");
                return result;
            }

            public void RoundLost()
            {
                var thread = new Thread(() =>
                controller.RoundLost());

                HandleThread(thread, "RoundLost");
            }

            public void RoundWon()
            {
                Register.Score++;
                controller.Register.Score++;

                var thread = new Thread(() =>
                controller.RoundWon());

                HandleThread(thread, "RoundWon");
            }
        }
    }
}