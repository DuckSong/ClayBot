using System.Drawing;

namespace ClayBot
{
    static class Static
    {
        public const int TIMEOUT_CLIENT = 30000;
        public const decimal SENSITIVITY = 0.1m;

        public static string[] SUPPORTED_LOCALES = new string[]
        {
            "en-AU",
            "en-GB",
            "en-US",
            "es-MX",
            "pt-BR"
        };

        public static string[] PROCESS_NAMES = new string[]
        {
            "lol.launcher.admin",
            "lol.launcher",
            "rads_user_kernel",
            "LoLLauncher",
            "LoLPatcher",
            "LolClient",
            "League of Legends",
            "BsSndRpt"
        };

        public const string PATCHER_CLASS_NAME = "LOLPATCHER";
        public const string CLIENT_CLASS_NAME = "ApolloRuntimeContentWindow";
        public const string GAME_CLASS_NAME = "RiotWindowClass";

        public const string CONFIG_PATH = @"Data\Config.xml";

        public static Size SMALL_PATCHER_SIZE = new Size(992, 620);
        public static Size LARGE_PATCHER_SIZE = new Size(1280, 800);
        public static Size CLIENT_SIZE = new Size(1024, 640);

        #region Patcher Click Points
        #endregion

        #region Patcher Drawing Rectangles
        
        #endregion

        #region Client Click Points
        public static Point LOGIN_USERNAME_POINT = new Point(100, 255);
        public static Point LOGIN_PASSWORD_POINT = new Point(100, 310);

        public static Point PVP_POINT = new Point(260, 100);
        public static Point ARAM_POINT = new Point(370, 145);
        public static Point HOWLING_ABYSS_POINT = new Point(535, 125);
        public static Point NORMAL_POINT = new Point(700, 125);
        #endregion

        #region Client Drawing Rectangles
        #endregion
    }
}
