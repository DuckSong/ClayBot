using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ClayBot
{
    public class Config
    {
        private const string LOL_LAUNCHER_PATH_DEFAULT = @"C:\Riot Games\League of Legends\lol.launcher.admin.exe";
        private const string LOL_LAUNCHER_EXECUTABLE_NAME = "lol.launcher.admin.exe";
        private const string LOL_REGION_DEFAULT = "North America";
        private const string LOL_LANGUAGE_DEFAULT = "English";
        private const string LOL_LOCALE_DEFAULT = "en-US";
        
        public string LolLauncherPath;
        public string LolRegion;
        public string LolLanguage;
        public string LolLocale;
        public string LolId;
        public string LolPassword;

        public Config()
        {
            LolLauncherPath = LOL_LAUNCHER_PATH_DEFAULT;
            LolRegion = LOL_REGION_DEFAULT;
            LolLanguage = LOL_LANGUAGE_DEFAULT;
            LolLocale = LOL_LOCALE_DEFAULT;
            LolId = string.Empty;
            LolPassword = string.Empty;
        }
        
        public bool GetLolLauncherPath()
        {
            if (!File.Exists(LolLauncherPath))
            {
                MessageBox.Show("Launcher not found at: " + LolLauncherPath);

                if (MessageBox.Show(
                    "Do you want me to automatically find it for you? (This might take some time.)",
                    "Find lol.launcher.admin.exe?",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    LolLauncherPath = FindLolLauncherPath();
                }

                return false;
            }

            return true;
        }

        private string FindLolLauncherPath()
        {
            foreach (DriveInfo driveInfo in DriveInfo.GetDrives().Where(x => x.IsReady))
            {
                string candidate = FindFile(driveInfo.RootDirectory.FullName, LOL_LAUNCHER_EXECUTABLE_NAME);

                if (!string.IsNullOrEmpty(candidate)) return candidate;
            }

            return string.Empty;
        }

        private string FindFile(string directory, string fileName)
        {
            try
            {
                foreach (string file in Directory.GetFiles(directory))
                {
                    if (Path.GetFileName(file) == fileName) return file;
                }
            } catch { }

            try
            {
                foreach (string subDirectory in Directory.GetDirectories(directory))
                {
                    string candidate = FindFile(subDirectory, fileName);

                    if (!string.IsNullOrEmpty(candidate)) return candidate;
                }
            } catch { }

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
