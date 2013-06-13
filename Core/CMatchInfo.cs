using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    ///     <item><b>mbc_game_mode</b> - "classic", "salvo", "powered", "classic multi", "salvo multi", "powered multi"
    ///     determines the game logic.
    ///     </item>
    /// </list>
    /// This class is an extension of the MatchInfo class, which cannot be constructed. This class provides
    /// a constructor to the MatchInfo class so it can have its values initialized. The reason for this is to
    /// prevent the need to place the Configuration class into the plugin DLL.
    /// </summary>
    /// <seealso cref="MatchInfo"/>
    public class CMatchInfo : MatchInfo
    {
        /// <summary>Sets default configuration values for keys that relate to this class.
        /// Should be called before using the global Configuration.Default object.</summary>
        /// <seealso cref="Configuration"/>
        public static void SetConfigDefaults()
        {
            Configuration.Default.SetValue<int>("mbc_field_width", 10);
            Configuration.Default.SetValue<int>("mbc_field_height", 10);
            Configuration.Default.SetValue<string>("mbc_ship_sizes", "2,3,3,4,5");
            Configuration.Default.SetValue<int>("mbc_timeout", 100);
            Configuration.Default.SetValue<string>("mbc_game_mode", "classic");
        }

        public CMatchInfo(Configuration config, params Controller.ClassInfo[] controllerInfos)
        {
            this.playerNames = new List<string>();
            foreach (var controllerInfo in controllerInfos)
            {
                this.playerNames.Add(controllerInfo.Name);
            }

            //Configuration setting for a list of ships that the controllers start with.
            initShips = new ShipList(config.GetList<int>("mbc_ship_sizes").ToArray());

            //Configuration setting for the size of the field.
            fieldSize = new Coordinates(
                config.GetValue<int>("mbc_field_width"),
                config.GetValue<int>("mbc_field_height"));

            //Configuration setting for the amount of time a controller is allowed per method invoke.
            methodTimeLimit = config.GetValue<int>("mbc_timeout");

            gameMode = GameMode.Classic;
            //Configuration setting for the kind of game mode this match will be set to.
            switch (config.GetValue<string>("mbc_game_mode"))
            {
                //Add the game modes here as they are implemented...
                case "classic":
                    gameMode = GameMode.Classic;
                    break;
                case "classic multi":
                    gameMode = GameMode.ClassicMulti;
                    break;
                default:
                    config.SetValue<string>("mbc_game_mode", "classic");
                    gameMode = GameMode.Classic;
                    break;
            }

            //Make sure all of the controllers are compatible with the given game mode.
            foreach (var controllerInfo in controllerInfos)
            {
                if (!controllerInfo.CompatibleModes.Contains(gameMode))
                {
                    throw new ControllerIncompatibleException(controllerInfo, gameMode);
                }
            }
        }
    }
}
