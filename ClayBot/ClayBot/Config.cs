using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;

namespace ClayBot
{
    public class Config
    {
        private const string LOL_DIRECTORY_DEFAULT = @"C:\Riot Games\League of Legends";
        private const string LOL_LAUNCHER_EXECUTABLE_NAME = "lol.launcher.admin.exe";
        private const string LOL_AIR_CLIENT_RELATIVE_DIRECTORY = @"RADS\projects\lol_air_client\releases";
        private const string LOL_LOCALE_PROPERTIES_RELATIVE_PATH = @"deploy\locale.properties";

        public string LolDirectory;
        public string LolLauncherPath;
        public string LolLocale;
        public string LolId;
        public string LolPassword;

        public Config()
        {
            LolDirectory = LOL_DIRECTORY_DEFAULT;
            LolId = string.Empty;
            LolPassword = string.Empty;
        }

        public void Initialize()
        {
            LolLauncherPath = GetLolLauncherPath();
            LolLocale = GetLolLocale();
        }

        private string GetLolLauncherPath()
        {
            if (string.IsNullOrEmpty(LolDirectory) || !Directory.Exists(LolDirectory)) return string.Empty;
            
            string lolLauncherPath = Path.Combine(LolDirectory, LOL_LAUNCHER_EXECUTABLE_NAME);

            return File.Exists(lolLauncherPath) ? lolLauncherPath : string.Empty;
        }

        private string GetLolLocale()
        {
            if (string.IsNullOrEmpty(LolDirectory) || !Directory.Exists(LolDirectory)) return string.Empty;

            string airClientDirectory = Path.Combine(LolDirectory, LOL_AIR_CLIENT_RELATIVE_DIRECTORY);
            if (!Directory.Exists(airClientDirectory)) return string.Empty;

            string airClientReleaseDirectory = Directory.GetDirectories(airClientDirectory).ElementAtOrDefault(0);
            if (string.IsNullOrEmpty(airClientReleaseDirectory)) return string.Empty;

            string lolLocalePath = Path.Combine(airClientReleaseDirectory, LOL_LOCALE_PROPERTIES_RELATIVE_PATH);
            if (!File.Exists(lolLocalePath)) return string.Empty;

            using (StreamReader localeReader = new StreamReader(lolLocalePath))
            {
                while (localeReader.Peek() != -1)
                {
                    string line = localeReader.ReadLine();

                    if (line.Contains("locale="))
                    {
                        string locale = line.Substring(line.IndexOf("=") + 1).Replace("_", "-").Trim();

                        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(locale);
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(locale);

                        return locale;
                    }
                }
            }

            return string.Empty;
        }

        public static Config ReadConfig()
        {
            string configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Static.CONFIG_PATH);

            if (!File.Exists(configPath)) WriteConfig(new Config());

            using (StreamReader configReader = new StreamReader(configPath))
            {
                return new XmlSerializer(typeof(Config)).Deserialize(configReader) as Config;
            }
        }

        public static void WriteConfig(Config config)
        {
            string configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Static.CONFIG_PATH);

            using (StreamWriter configWriter = new StreamWriter(configPath, false))
            {
                new XmlSerializer(typeof(Config)).Serialize(configWriter, config);
            }
        }
    }
}
