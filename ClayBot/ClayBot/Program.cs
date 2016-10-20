using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ClayBot
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            string DataDirectory = Path.GetDirectoryName(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Static.CONFIG_PATH));
            if (!Directory.Exists(DataDirectory)) Directory.CreateDirectory(DataDirectory);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm mainForm = new MainForm();
            if (mainForm.Initialize())
            {
                Application.Run(mainForm);
            }
        }
    }
}
