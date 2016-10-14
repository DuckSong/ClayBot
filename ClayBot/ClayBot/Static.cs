using System.Collections.Generic;
using System.Drawing;

namespace ClayBot
{
    enum PatcherSize
    {
        Small,
        Large
    }

    enum PatcherRectangle
    {
        PatcherIndicator,
        Launch,
        Online,
        Accept
    }

    static class Static
    {
        public const int ACTIVATE_WINDOW_DELAY = 1000;
        public const int CURSOR_CLICK_DELAY = 500;
        public const int KEY_ENTER_DELAY = 500;
        public const int TIMEOUT_CLIENT = 30000;
        public const double SENSITIVITY = 0.1d;
        public const double THRESHOLD_GRAY = 200d;

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

        public static Dictionary<PatcherSize, Size> PATCHER_SIZES = new Dictionary<PatcherSize, Size>()
        {
            { PatcherSize.Small, new Size(992, 620) },
            { PatcherSize.Large, new Size(1280, 800) }
        };
        public static Size CLIENT_SIZE = new Size(1024, 640);

        public static Dictionary<PatcherRectangle, Dictionary<PatcherSize, Rectangle>> PATCHER_RECTANGLES = new Dictionary<PatcherRectangle, Dictionary<PatcherSize, Rectangle>>()
        {
            {
                PatcherRectangle.PatcherIndicator,
                new Dictionary<PatcherSize, Rectangle>()
                {
                    { PatcherSize.Small, new Rectangle(20, 10, 91, 32) },
                    { PatcherSize.Large, new Rectangle(25, 11, 116, 44) }
                }
            },
            {
                PatcherRectangle.Launch,
                new Dictionary<PatcherSize, Rectangle>()
                {
                    { PatcherSize.Small, new Rectangle(441, 11, 108, 36) },
                    { PatcherSize.Large, new Rectangle(569, 11, 140, 45) }
                }
            },
            {
                PatcherRectangle.Online,
                new Dictionary<PatcherSize, Rectangle>()
                {
                    { PatcherSize.Small, new Rectangle(945, 26, 38, 15) },
                    { PatcherSize.Large, new Rectangle(1220, 28, 46, 16) }
                }
            },
            {
                PatcherRectangle.Accept,
                new Dictionary<PatcherSize, Rectangle>()
                {
                    { PatcherSize.Small, new Rectangle(336, 520, 53, 17) },
                    { PatcherSize.Large, new Rectangle(478, 608, 55, 20) }
                }
            }
        };
    }
}
