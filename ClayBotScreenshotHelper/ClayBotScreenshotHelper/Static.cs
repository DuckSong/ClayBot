using System.Collections.Generic;
using System.Drawing;

namespace ClayBotScreenshotHelper
{
    static class Static
    {
        public static double THRESHOLD_GRAY = 200d;

        public static Dictionary<string, string> CLASS_NAMES = new Dictionary<string, string>()
        {
            { "Patcher", "LOLPATCHER" },
            { "Client", "ApolloRuntimeContentWindow" },
            { "Game", "RiotWindowClass" }
        };

        public static Dictionary<PatcherSize, Size> PATCHER_SIZE = new Dictionary<PatcherSize, Size>()
        {
            { PatcherSize.Small, new Size(992, 620) },
            { PatcherSize.Large, new Size(1280, 800) }
        };

        public static Size CLIENT_SIZE = new Size(1024, 640);
    }
}
