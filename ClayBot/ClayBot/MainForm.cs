using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static ClayBot.NativeMethods;

namespace ClayBot
{
    partial class MainForm : Form
    {
        private Label instructionLabel;
        private Label statusLabel;

        private IContainer components;

        public MainForm() : base()
        {
            components = new Container();
        }

        public void SetStatus(string status, bool bad = false)
        {
            Invoke((MethodInvoker)delegate
            {
                instructionLabel.Text = Strings.Strings.Instruction;
                statusLabel.Text = status;
                statusLabel.ForeColor = bad ? Color.Red : Color.Green;

                ResizeWindow();
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (workerThread != null && mainWorker != null)
                {
                    workerThread.Abort();
                    workerThread.Join();
                }

                base.Dispose(disposing);
            }
        }

        private void CreateForm(Control.ControlCollection controlCollection, Point startPoint, int padding, bool isMainForm, params Control[] controls)
        {
            for (int i = 0; i < controls.Length; i++)
            {
                if (isMainForm) components.Add(controls[i]);
                controlCollection.Add(controls[i]);
                controls[i].Location = i == 0 ? startPoint : new Point(startPoint.X, controls[i - 1].Bottom + padding);
            }
        }

        public bool Initialize()
        {
            if (!ValidateDPI()) return false;

            SuspendLayout();

            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            TopMost = false;
            Location = Screen.PrimaryScreen.WorkingArea.Location;

            CreateForm(Controls, new Point(5, 5), 5, true,
                instructionLabel = new Label()
                {
                    AutoSize = true,
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = "",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font(Font.Name, 15)
                },
                statusLabel = new Label()
                {
                    AutoSize = true,
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = "",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font(Font.Name, 15)
                });

            ResumeLayout();

            InitializeConfig();
            InitializeWorker();
            InitializeInput();

            return true;
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

        private bool ValidateDPI()
        {
            float scale = 0.0f;

            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                IntPtr desktopDc = g.GetHdc();

                scale = (float)GetDeviceCaps(desktopDc, DeviceCap.DESKTOPVERTRES) / GetDeviceCaps(desktopDc, DeviceCap.VERTRES);

                g.ReleaseHdc();
            }
            
            if (scale != 1.0f)
            {
                MessageBox.Show(Strings.Strings.DPIWarning);

                return false;
            }

            return true;
        }
    }
}
