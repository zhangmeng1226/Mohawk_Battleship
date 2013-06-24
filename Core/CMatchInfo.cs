using MBC.Core.Util;
using MBC.Shared;
using System;
using System.Collections.Generic;

namespace MBC.Core
{
    /// <summary>
    /// Derives a <see cref="MatchInfo"/> for the sole purpose of initializing its field members.
    /// </summary>
    /// <seealso cref="MatchInfo"/>
    [Configuration("mbc_field_width", 10)]
    [Configuration("mbc_field_height", 10)]
    [Configuration("mbc_ship_sizes", "2,3,3,4,5")]
    [Configuration("mbc_timeout", 200)]
    [Configuration("mbc_game_mode", "classic")]
    public class CMatchInfo : MatchInfo
    {
        /// <summary>
        /// Initializes the underlying <see cref="MatchInfo"/> with values provided by a <see cref="Configuration"/>
        /// and associates the <see cref="MatchInfo"/> with the given <see cref="ControllerInformation"/>.
        /// </summary>
        /// <param name="config">The <see cref="Configuration"/> to load settings from.</param>
        /// <param name="controllerInfos">A variable number of <see cref="ControllerInformation"/>
        /// to associate with.</param>
        /// <exception cref="ControllerIncompatibleException">A controller is not designed for the
        /// configured <see cref="GameMode"/>.</exception>
        public CMatchInfo(Configuration config, params ControllerInformation[] controllerInfos)
        {
            //Get the game mode from the Configuration.
            DetermineGameMode(config);

            //Make sure all of the controllers are compatible with the given game mode.
            foreach (var controllerInfo in controllerInfos)
            {
                if (!controllerInfo.Capabilities.CompatibleWith(gameMode))
                {
                    throw new ControllerIncompatibleException(controllerInfo, gameMode);
                }
            }

            this.controllerNames = new List<string>();
            foreach (var controllerInfo in controllerInfos)
            {
                this.controllerNames.Add(controllerInfo.ToString());
            }

            //Configuration setting for a list of ships that the controllers start with.
            initShips = new ShipList(config.GetList<int>("mbc_ship_sizes").ToArray());

            //Configuration setting for the size of the field.
            fieldSize = new Coordinates(
                config.GetValue<int>("mbc_field_width"),
                config.GetValue<int>("mbc_field_height"));

            //Configuration setting for the amount of time a controller is allowed per method invoke.
            methodTimeLimit = config.GetValue<int>("mbc_timeout");
        }

        private void DetermineGameMode(Configuration config)
        {
            string[] gameModeSplit = config.GetValue<string>("mbc_game_mode").Split(' ');
            gameMode = 0;
            foreach (var gmStr in gameModeSplit)
            {
                switch (gmStr)
                {
                    case "classic":
                        gameMode |= GameMode.Classic;
                        break;

                    case "salvo":
                        gameMode |= GameMode.Salvo;
                        throw new NotImplementedException("Salvo game mode does not exist yet.");
                    case "powered":
                        gameMode |= GameMode.Powered;
                        throw new NotImplementedException("Powered game mode does not exist yet.");
                    case "multi":
                        gameMode |= GameMode.Multi;
                        break;

                    case "teams":
                        gameMode |= GameMode.Teams;
                        throw new NotImplementedException("Teams game mode does not exist yet.");
                }
            }
        }
    }
}