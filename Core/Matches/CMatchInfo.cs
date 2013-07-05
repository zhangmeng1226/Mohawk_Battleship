using MBC.Core.Util;
using MBC.Shared;
using System;
using System.Collections.Generic;

namespace MBC.Core.Matches
{
    /// <summary>
    /// Derives a <see cref="MatchInfo"/> for the sole purpose of initializing its field members.
    /// </summary>
    /// <seealso cref="MatchInfo"/>
    [Configuration("mbc_field_width", 10)]
    [Configuration("mbc_field_height", 10)]
    [Configuration("mbc_ship_sizes", 2, 3, 3, 4, 5)]
    [Configuration("mbc_timeout", 200)]
    [Configuration("mbc_game_mode", GameMode.Classic, null)]
    public class CMatchInfo : MatchInfo
    {
        /// <summary>
        /// Initializes the underlying <see cref="MatchInfo"/> with values provided by a <see cref="Configuration"/>
        /// and associates the <see cref="MatchInfo"/> with the given <see cref="ControllerInformation"/>.
        /// </summary>
        /// <param name="config">The <see cref="Configuration"/> to load settings from.</param>
        /// <param name="controllerNames">A variable number of <see cref="ControllerInformation"/>
        /// to associate with.</param>
        /// <exception cref="ControllerIncompatibleException">A controller is not designed for the
        /// configured <see cref="GameMode"/>.</exception>
        public CMatchInfo(Configuration config)
        {
            //Get the game mode from the Configuration.
            DetermineGameMode(config);

            this.controllerNames = new List<string>();

            //Configuration setting for a list of ships that the controllers start with.
            initShips = new ShipList(config.GetList<int>("mbc_ship_sizes").ToArray());

            //Configuration setting for the size of the field.
            fieldSize = new Coordinates(
                config.GetValue<int>("mbc_field_width"),
                config.GetValue<int>("mbc_field_height"));

            //Configuration setting for the amount of time a controller is allowed per method invoke.
            methodTimeLimit = config.GetValue<int>("mbc_timeout");
        }

        public void AddControllerName(string name)
        {
            controllerNames.Add(name);
        }

        private void DetermineGameMode(Configuration config)
        {
            gameMode = 0;
            foreach (var gmStr in config.GetValue<List<GameMode>>("mbc_game_mode"))
            {
                gameMode |= gmStr;
                if (gmStr == GameMode.Salvo || gmStr == GameMode.Powered || gmStr == GameMode.Teams)
                {
                    throw new NotImplementedException("The "+gmStr.ToString()+" game mode is not supported.");
                }
            }
        }
    }
}