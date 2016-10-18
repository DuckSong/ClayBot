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

    enum ClientRectangle
    {
        LoginIndicator,
        LoginIndicator2,
        LoginIndicatorThreshold,
        Login,
        InvalidLoginOk,
        LoggingInIndicatorThreshold,
        Play,
        Reconnect,
        LeaverBusterWarning,
        InactivePlay,
        AramIndicator,
        Solo,
        JoinQueueFailedIndicator,
        LeaverBusterIndicator,
        InQueueIndicator,
        Accept,
        TeamChat,
        Home
    }

    enum ClientClickPoint
    {
        LoginUsername,
        LoginPassword,
        LeaverBusterWarning,
        LeaverBusterOk,
        SelectPvp,
        SelectAram,
        SelectHowlingAbyss,
        SelectNormal
    }

    static class Static
    {
        public const int ACTIVATE_WINDOW_DELAY = 1000;
        public const int CURSOR_CLICK_DELAY = 500;
        public const int KEY_ENTER_DELAY = 250;
        public const int TIMEOUT_CLIENT = 10000;
        public const int RETRY = 5;
        public const double SENSITIVITY = 0.1d;
        public const double THRESHOLD_GRAY = 200d;
        
        public struct Locale
        {
            public string Region;
            public string Language;
            public string LocaleCode;

            public Locale(string region, string language, string localeCode)
            {
                Region = region;
                Language = language;
                LocaleCode = localeCode;
            }
        }

        public static Locale[] SUPPORTED_LOCALES = new Locale[]
        {
            new Locale("North America", "English", "en-US"),
            new Locale("EU West", "English", "en-GB"),
            new Locale("EU Nordic & East", "English", "en-GB"),
            new Locale("Latin America North", "Spanish", "es-MX"),
            new Locale("Latin America South", "Spanish", "es-MX"),
            new Locale("Brazil", "Portuguese", "pt-BR"),
            new Locale("Japan", "Japanese", "ja-JA"),
            new Locale("Russia", "Russian", "ru-RU"),
            new Locale("Turkey", "Turkish", "tr-TR"),
            new Locale("Oceania", "English", "en-AU"),
            new Locale("Republic of Korea", "Korean", "ko-KO")
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

        public static Dictionary<ClientRectangle, Rectangle> CLIENT_RECTANGLES = new Dictionary<ClientRectangle, Rectangle>()
        {
            { ClientRectangle.LoginIndicator, new Rectangle(106, 90, 202, 65) },
            { ClientRectangle.LoginIndicator2, new Rectangle(24, 557, 41, 21) },
            { ClientRectangle.LoginIndicatorThreshold, new Rectangle(72, 195, 84, 17) },
            { ClientRectangle.Login, new Rectangle(276, 333, 33, 16) },
            { ClientRectangle.InvalidLoginOk, new Rectangle(554, 420, 23, 12) },
            { ClientRectangle.LoggingInIndicatorThreshold, new Rectangle(377, 237, 75, 15) },
            { ClientRectangle.Reconnect, new Rectangle(481, 357, 62, 13) },
            { ClientRectangle.Play, new Rectangle(449, 13, 118, 35) },
            { ClientRectangle.LeaverBusterWarning, new Rectangle(321, 193, 164, 21) },
            { ClientRectangle.InactivePlay, new Rectangle(450, 14, 116, 40) },
            { ClientRectangle.AramIndicator, new Rectangle(200, 266, 292, 136) },
            { ClientRectangle.Solo, new Rectangle(575, 560, 47, 17) },
            { ClientRectangle.JoinQueueFailedIndicator, new Rectangle(222, 203, 198, 19) },
            { ClientRectangle.LeaverBusterIndicator, new Rectangle(222, 193, 92, 21) },
            { ClientRectangle.InQueueIndicator, new Rectangle(450, 6, 114, 12) },
            { ClientRectangle.Accept, new Rectangle(396, 357, 45, 17) },
            { ClientRectangle.TeamChat, new Rectangle(227, 449, 63, 17) },
            { ClientRectangle.Home, new Rectangle(708, 563, 38, 19) }
        };

        public static Dictionary<ClientClickPoint, Point> CLIENT_CLICK_POINTS = new Dictionary<ClientClickPoint, Point>()
        {
            { ClientClickPoint.LoginUsername, new Point(100, 255) },
            { ClientClickPoint.LoginPassword, new Point(100, 310) },
            { ClientClickPoint.LeaverBusterWarning, new Point(347, 400) },
            { ClientClickPoint.LeaverBusterOk, new Point(511, 428) },
            { ClientClickPoint.SelectPvp, new Point(260, 100) },
            { ClientClickPoint.SelectAram, new Point(370, 145) },
            { ClientClickPoint.SelectHowlingAbyss, new Point(535, 125) },
            { ClientClickPoint.SelectNormal, new Point(700, 125) }
        };
    }
}
