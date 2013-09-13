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
    public class ConfiguredMatchInfo : MatchInfo
    {

        protected ConfiguredMatchInfo() { }

        public Configuration Config
        {
            get;
            internal set;
        }

        public ConfiguredMatchInfo(Configuration config)
        {
            DetermineGameMode(config);

            controllerNames = new List<string>();

            initShips = new ShipList(config.GetList<int>("mbc_ship_sizes").ToArray());

            fieldSize = new Coordinates(
                config.GetValue<int>("mbc_field_width"),
                config.GetValue<int>("mbc_field_height"));

            methodTimeLimit = config.GetValue<int>("mbc_timeout");
        }

        public void AddControllerName(string name)
        {
            controllerNames.Add(name);
        }

        private void DetermineGameMode(Configuration config)
        {
            gameMode = 0;
            foreach (var gmStr in config.GetList<GameMode>("mbc_game_mode"))
            {
                gameMode |= gmStr;
                if (gmStr == GameMode.Salvo || gmStr == GameMode.Powered || gmStr == GameMode.Teams)
                {
                    throw new NotImplementedException("The " + gmStr.ToString() + " game mode is not supported.");
                }
            }
        }
    }
}