using MBC.Core.Util;

namespace MBC.App.BattleshipConsole
{
    public static class Load
    {
        public static void Config(int idx, params string[] param)
        {
            Configuration loadConf = new Configuration(param[idx++]);
        }

        public static void Match(int idx, params string[] param)
        {
        }
    }
}