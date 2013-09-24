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
    public class BattleshipUser
    {
        private Controller controller;

        private ControllerInformation controllerInfo;

        private MultiMethodThreader threader;

        public BattleshipUser(ControllerInformation targetControllerInfo)
        {
            Register.Name = targetControllerInfo.Name;
            controllerInfo = targetControllerInfo;

            controller = (Controller)Activator.CreateInstance(targetControllerInfo.Controller);
            controller.ControllerMessageEvent += ReceiveMessage;

            threader = new MultiMethodThreader();
            threader["GetShot"] = () => new PlayerShotEvent(this, controller.MakeShot());
            threader["MatchOver"] = () => controller.MatchOver();
            threader["NewMatch"] = () => controller.NewMatch();
            threader["NewRound"] = () => controller.NewRound();
            threader["OpponentShot"] = null; //Created on the fly
            threader["ShotHit"] = null; //Created on the fly
            threader["ShotMiss"] = null; //Created on the fly
            threader["PlaceShips"] = () => new PlayerShipsPlacedEvent(this, controller.PlaceShips());
            threader["RoundLost"] = () => controller.RoundLost();
            threader["RoundWon"] = () => controller.RoundWon();
        }

        /// <summary>
        /// Occurs whenever the <see cref="Controller"/> outputs a message string.
        /// </summary>
        public event StringOutputHandler ControllerMessageEvent;

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
