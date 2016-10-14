using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ClayBot.StateMachine
{
    partial class MainWorker
    {
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

            public static implicit operator Rectangle(RECT r)
            {
                return new Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                    return Equals((RECT)obj);
                else if (obj is Rectangle)
                    return Equals(new RECT((Rectangle)obj));
                else
                    return false;
            }

            public override int GetHashCode()
            {
                return ((Rectangle)this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }

        struct Window
        {
            public IntPtr Handle;
            public string ClassName;
            public string Text;
            public RECT Rect;
            public bool Found;
        }

        const uint SHOW_WINDOW_RESTORE = 9;
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;
        const int KEYEVENTF_KEYUP = 0x02;

        private Window targetWindow;
        private EnumWindowsProc enumWindowsDelegate;
        
        private bool FindWindow(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder classNameBuilder = new StringBuilder(1024);
            GetClassName(hWnd, classNameBuilder, 1024);
            string className = classNameBuilder.ToString();

            if (className == targetWindow.ClassName)
            {
                StringBuilder textBuilder = new StringBuilder(1024);
                GetWindowText(hWnd, textBuilder, 1024);
                string text = textBuilder.ToString();

                if (text == targetWindow.Text)
                {
                    targetWindow.Handle = hWnd;
                    targetWindow.Rect = new RECT();
                    GetWindowRect(hWnd, out targetWindow.Rect);
                    targetWindow.Found = true;

                    return false;
                }
            }

            EnumChildWindows(hWnd, enumWindowsDelegate, lParam);

            return true;
        }

        private bool FindWindow(string className, string text)
        {
            targetWindow = new Window()
            {
                ClassName = className,
                Text = text,
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

                Thread.Sleep(Static.ACTIVATE_WINDOW_DELAY);
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
        
        private void Click(int x, int y)
        {
            SetCursorPos(x, y);
            Thread.Sleep(Static.CURSOR_CLICK_DELAY);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(Static.CURSOR_CLICK_DELAY);
        }

        private void ClickTargetWindowRectangle(Rectangle target)
        {
            Click(
                targetWindow.Rect.Left + target.Left + (target.Width / 2),
                targetWindow.Rect.Top + target.Top + (target.Height / 2));
        }

        private void SendText(string text)
        {
            foreach (char c in text)
            {
                bool isUpper = c >= 'A' && c <= 'Z';

                if (isUpper)
                {
                    keybd_event((byte)Keys.LShiftKey, 0, 0, UIntPtr.Zero);
                    Thread.Sleep(Static.KEY_ENTER_DELAY);
                }

                byte key = (byte)(c.ToString().ToUpper()[0] - 'A' + Keys.A);
                
                keybd_event(key, 0, 0, UIntPtr.Zero);
                Thread.Sleep(Static.KEY_ENTER_DELAY);
                keybd_event(key, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);

                if (isUpper)
                {
                    Thread.Sleep(Static.KEY_ENTER_DELAY);
                    keybd_event((byte)Keys.LShiftKey, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                }
            }
        }

        private void EmptyTextBox()
        {
            keybd_event((byte)Keys.LControlKey, 0, 0, UIntPtr.Zero);
            Thread.Sleep(Static.KEY_ENTER_DELAY);
            keybd_event((byte)Keys.A, 0, 0, UIntPtr.Zero);
            Thread.Sleep(Static.KEY_ENTER_DELAY);
            keybd_event((byte)Keys.A, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            Thread.Sleep(Static.KEY_ENTER_DELAY);
            keybd_event((byte)Keys.LControlKey, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            Thread.Sleep(Static.KEY_ENTER_DELAY);
            keybd_event((byte)Keys.Back, 0, 0, UIntPtr.Zero);
            Thread.Sleep(Static.KEY_ENTER_DELAY);
            keybd_event((byte)Keys.Back, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        private bool CheckPatcher(out PatcherSize patcherSize)
        {
            patcherSize = PatcherSize.Small;

            if (!FindWindow(Static.PATCHER_CLASS_NAME, Strings.Strings.PatcherText)) return false;

            foreach (PatcherSize candidateSize in Enum.GetValues(typeof(PatcherSize)) as PatcherSize[])
            {
                if (targetWindow.Rect.Size == Static.PATCHER_SIZES[candidateSize])
                {
                    patcherSize = candidateSize;
                    break;
                }
            }

            return true;
        }
        
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

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bvk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

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
    }
}