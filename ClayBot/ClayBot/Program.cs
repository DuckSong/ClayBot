using System;
using System.IO;
using System.Reflection;
using System.Threading;
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
            
            Application.ThreadException += new ThreadExceptionEventHandler(ThreadExceptionHandler);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm mainForm = new MainForm();

            if (mainForm.Initialize())
            {
                Application.Run(mainForm);
            }
        }

        static void ThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            using (StreamWriter errorWriter = new StreamWriter(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "error.log"), false))
            {
                errorWriter.Write(e.Exception.ToString());
            }
        }

        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            using (StreamWriter errorWriter = new StreamWriter(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "error.log"), false))
            {
                errorWriter.Write(e.ExceptionObject.ToString());
            }
        }
    }
}
