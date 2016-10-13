using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ClayBot.MainForm
{
    partial class MainForm : Form
    {
        private Label instructionLabel;
        private Label statusLabel;

        private IContainer components;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            if (workerThread != null && mainWorker != null)
            {
                mainWorker.Dispose();
                workerThread.Join();
            }

            base.Dispose(disposing);
        }

        public MainForm()
        {
            components = new Container();

            Initialize();
            InitializeConfig();
            InitializeWorker();
        }
        
        private void Initialize()
        {
            SuspendLayout();

            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            TopMost = true;

            Rectangle primaryScreenWorkingArea = Screen.PrimaryScreen.WorkingArea;
            Location = primaryScreenWorkingArea.Location;
            
            instructionLabel = new Label()
            {
                AutoSize = true,
                BorderStyle = BorderStyle.FixedSingle,
                Text = "",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(Font.Name, 15)
            };
            components.Add(instructionLabel);
            Controls.Add(instructionLabel);
            instructionLabel.Location = new Point(5, 5);

            statusLabel = new Label()
            {
                AutoSize = true,
                BorderStyle = BorderStyle.FixedSingle,
                Text = "",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(Font.Name, 15)
            };
            components.Add(statusLabel);
            Controls.Add(statusLabel);
            statusLabel.Location = new Point(5, instructionLabel.Bottom + 5);

            ResumeLayout();
        }

        private void ResizeWindow()
        {
            ClientSize = new Size(
                new int[]
                {
                    instructionLabel.Width,
                    statusLabel.Width
                }.Max() + 10, statusLabel.Bottom + 5);
        }

        public void SetStatus(string status, bool bad = false)
        {
            Invoke((MethodInvoker)delegate
            {
                instructionLabel.Text = Strings
                statusLabel.Text = status;
                statusLabel.ForeColor = bad ? Color.Red : Color.Green;

                ResizeWindow();
            });
        }
    }
}
