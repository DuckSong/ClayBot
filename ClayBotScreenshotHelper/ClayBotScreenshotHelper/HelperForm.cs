using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ClayBotScreenshotHelper
{
    enum PatcherSize
    {
        Small,
        Large
    };

    struct Window
    {
        public IntPtr Handle;
        public string ClassName;
        public RECT Rect;
        public bool Found;
    }

    #region P/Invoke
    [StructLayout(LayoutKind.Sequential)]
    struct RECT
    {
        public int Left, Top, Right, Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

        public int X
        {
            get { return Left; }
            set { Right -= (Left - value); Left = value; }
        }

        public int Y
        {
            get { return Top; }
            set { Bottom -= (Top - value); Top = value; }
        }

        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }

        public Point Location
        {
            get { return new Point(Left, Top); }
            set { X = value.X; Y = value.Y; }
        }

        public Size Size
        {
            get { return new Size(Width, Height); }
            set { Width = value.Width; Height = value.Height; }
        }
    }
    #endregion

    public partial class HelperForm : Form
    {
        private const uint SHOW_WINDOW_RESTORE = 9;

        private EnumWindowsProc enumWindowsDelegate;
        private Window targetWindow;

        public HelperForm()
        {
            InitializeComponent();
        }

        private void savePathButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                DefaultExt = "png",
                Filter = "PNG|*.png"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                savePathTextBox.Text = saveFileDialog.FileName;
            }
        }

        private void screenshotButton_Click(object sender, EventArgs e)
        {
            screenshotButton.Enabled = false;

            if (FindWindow(Static.CLASS_NAMES[targetComboBox.SelectedItem as string]))
            {
                switch (targetComboBox.SelectedItem as string)
                {
                    case "Patcher":
                        ActivateTargetWindow(Static.PATCHER_SIZE[DeterminePatcherSize()]);
                        break;
                    case "Client":
                        ActivateTargetWindow(Static.CLIENT_SIZE);
                        break;
                    case "Game":
                        ActivateTargetWindow();
                        break;
                }

                Thread.Sleep(5000);

                StringBuilder text = new StringBuilder(1024);
                GetWindowText(targetWindow.Handle, text, 1024);
                new PictureForm(TakeTargetWindowScreenshot(), savePathTextBox.Text, text.ToString()).ShowDialog();
            }

            screenshotButton.Enabled = true;
        }

        private bool FindWindow(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder classNameBuilder = new StringBuilder(1024);
            GetClassName(hWnd, classNameBuilder, 1024);
            string className = classNameBuilder.ToString();

            if (className == targetWindow.ClassName)
            {
                targetWindow.Handle = hWnd;
                targetWindow.Rect = new RECT();
                GetWindowRect(hWnd, out targetWindow.Rect);
                targetWindow.Found = true;

                return false;
            }

            EnumChildWindows(hWnd, enumWindowsDelegate, lParam);

            return true;
        }

        private bool FindWindow(string className)
        {
            targetWindow = new Window()
            {
                ClassName = className,
                Found = false
            };

            enumWindowsDelegate = new EnumWindowsProc(FindWindow);
            EnumWindows(enumWindowsDelegate, IntPtr.Zero);

            return targetWindow.Found;
        }

        private void ActivateTargetWindow()
        {
            if (targetWindow.Handle != GetForegroundWindow())
            {
                if (IsIconic(targetWindow.Handle))
                {
                    ShowWindow(targetWindow.Handle, SHOW_WINDOW_RESTORE);
                }

                keybd_event(0, 0, 0, UIntPtr.Zero);

                SetForegroundWindow(targetWindow.Handle);
            }
        }

        private void ActivateTargetWindow(Size targetSize)
        {
            ActivateTargetWindow();

            Rectangle primaryScreenWorkingArea = Screen.PrimaryScreen.WorkingArea;
            MoveWindow(
                targetWindow.Handle,
                primaryScreenWorkingArea.Right - targetSize.Width,
                primaryScreenWorkingArea.Bottom - targetSize.Height,
                targetSize.Width,
                targetSize.Height,
                true);
            GetWindowRect(targetWindow.Handle, out targetWindow.Rect);
        }

        private PatcherSize DeterminePatcherSize()
        {
            foreach (PatcherSize patcherSize in Enum.GetValues(typeof(PatcherSize)) as PatcherSize[])
            {
                if (targetWindow.Rect.Size == Static.PATCHER_SIZE[patcherSize])
                {
                    return patcherSize;
                }
            }

            throw new Exception("Patcher size not found!");
        }

        private Bitmap TakeTargetWindowScreenshot()
        {
            Bitmap newBitmap = new Bitmap(targetWindow.Rect.Width, targetWindow.Rect.Height);

            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.CopyFromScreen(targetWindow.Rect.Location, Point.Empty, targetWindow.Rect.Size);

                if (thresholdCheckBox.Checked)
                {
                    using (Image<Bgr, byte> image = new Image<Bgr, byte>(newBitmap))
                    {
                        return image.Convert<Gray, byte>().ThresholdBinary(new Gray(Static.THRESHOLD_GRAY), new Gray(255d)).ToBitmap();
                    }
                }
                else
                {
                    return newBitmap;
                }
            }
        }

        #region P/Invoke
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bvk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        #endregion
    }
}
