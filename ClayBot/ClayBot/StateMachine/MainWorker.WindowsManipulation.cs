using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static ClayBot.NativeMethods;

namespace ClayBot.StateMachine
{
    partial class MainWorker
    {
        struct Window
        {
            public IntPtr Handle;
            public string ClassName;
            public string Text;
            public RECT Rect;
            public bool Found;
        }
        
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

        private void ClickTarget(int x, int y)
        {
            Click(targetWindow.Rect.Left + x, targetWindow.Rect.Top + y);
        }

        private void ClickTarget(Point p)
        {
            ClickTarget(p.X, p.Y);
        }

        private void ClickTargetWindowRectangle(Rectangle target)
        {
            ClickTarget(target.Left + (target.Width / 2), target.Top + (target.Height / 2));
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
    }
}