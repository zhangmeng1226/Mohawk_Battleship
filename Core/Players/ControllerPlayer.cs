using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core
{
    public class ControllerPlayer : Player
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
