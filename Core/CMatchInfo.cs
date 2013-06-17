using MBC.Core.Util;
using MBC.Shared;
using System;
using System.Collections.Generic;

namespace MBC.Core
{
    /// <summary>
    /// Contains information about the behaviour of the game logic that will be played in a Round.
    /// CMatchInfo objects use a Configuration object to set the game settings. The available options are the
    /// following:
    /// <list type="bullet">
    ///     <item><b>mbc_field_width</b> and <b>mbc_field_height</b> - Sets the boundaries of the field.</item>
    ///     <item><b>mbc_ship_sizes</b> - CSV of the sizes of ships that will be available.</item>
    ///     <item><b>mbc_timeout</b> - The time in milliseconds of the maximum amount of time a method call from a controller interface will have
    ///     before they lose the Round.
    ///     </item>
    ///     <item><b>mbc_game_mode</b> - "classic", "salvo", or "powered", followed with a space, "multi" or "teams"
    ///     determines the game logic. eg, "salvo multi teams" or "classic multi".
    ///     </item>
    /// </list>
    /// This class is an extension of the MatchInfo class, which cannot be constructed. This class provides
    /// a constructor to the MatchInfo class so it can have its values initialized. The reason for this is to
    /// prevent the need to place the Configuration class into the plugin DLL.
    /// </summary>
    /// <seealso cref="MatchInfo"/>
    [Configuration("mbc_field_width", 10)]
    [Configuration("mbc_field_height", 10)]
    [Configuration("mbc_ship_sizes", "2,3,3,4,5")]
    [Configuration("mbc_timeout", 200)]
    [Configuration("mbc_game_mode", "classic")]
    public class CMatchInfo : MatchInfo
    {
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

            this.playerNames = new List<string>();
            foreach (var controllerInfo in controllerInfos)
            {
                this.playerNames.Add(controllerInfo.ToString());
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