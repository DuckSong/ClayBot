using ClayBot.StateMachine;
using System.Threading;
using System.Windows.Forms;

namespace ClayBot
{
    partial class MainForm : Form
    {
        private Thread workerThread;
        private MainWorker mainWorker;

        private void InitializeWorker()
        {
            mainWorker = new MainWorker(this);

            workerThread = new Thread(new ThreadStart(mainWorker.Work));
            workerThread.Start();
        }
    }
}
