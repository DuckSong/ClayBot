using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static ClayBot.NativeMethods;

namespace ClayBot
{
    partial class MainForm : Form
    {
        private IntPtr hhk;
        private LowLevelKeyboardProc callbackDelegate;

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                switch ((Keys)Marshal.ReadInt32(lParam))
                {
                    case Keys.Escape:
                        UnhookWindowsHookEx(hhk);
                        Application.Exit();
                        break;
                    case Keys.F12:
                        Pause();
                        break;
                }
            }

            return CallNextHookEx(hhk, nCode, wParam, lParam);
        }

        private void InitializeInput()
        {
            callbackDelegate = new LowLevelKeyboardProc(HookCallback);
            hhk = SetWindowsHookEx(
                WH_KEYBOARD_LL,
                callbackDelegate,
                GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName),
                0);
        }

        public void Pause()
        {
            Invoke((MethodInvoker)delegate
            {
                workerThread.Abort();
                workerThread.Join();

                ShowConfigForm();

                InitializeWorker();
            });
        }
    }
}
