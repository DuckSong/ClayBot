using System.IO;
using System.Reflection;
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
            LolLauncherPath = string.Empty;
            LolLocale = string.Empty;

            if (!string.IsNullOrEmpty(LolDirectory) && Directory.Exists(LolDirectory))
            {
                LolLauncherPath = Path.Combine(LolDirectory, LOL_LAUNCHER_EXECUTABLE_NAME);

                if (!File.Exists(LolLauncherPath))
                {
                    LolLauncherPath = string.Empty;
                }

                string airClientDirectory = Path.Combine(LolDirectory, LOL_AIR_CLIENT_RELATIVE_DIRECTORY);

                if (Directory.Exists(airClientDirectory))
                {
                    string[] airClientReleaseDirectories = Directory.GetDirectories(airClientDirectory);

                    if (airClientReleaseDirectories.Length > 0)
                    {
                        string lolLocalePath = Path.Combine(airClientReleaseDirectories[0], LOL_LOCALE_PROPERTIES_RELATIVE_PATH);

                        if (File.Exists(lolLocalePath))
                        {
                            using (StreamReader localeReader = new StreamReader(lolLocalePath))
                            {
                                while (localeReader.Peek() != -1)
                                {
                                    string line = localeReader.ReadLine();

                                    if (line.Contains("locale="))
                                    {
                                        LolLocale = line.Substring(line.IndexOf("=") + 1).Replace("_", "-").Trim();
                                    }
                                }
                            }
                        }
                    }
                }
            }
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
