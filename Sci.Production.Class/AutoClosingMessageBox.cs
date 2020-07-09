using System;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// AutoClosingMessageBox
    /// </summary>
    public class AutoClosingMessageBox
    {
        private const int WM_CLOSE = 0x0010;
        private System.Threading.Timer _timeoutTimer;
        private string _caption;

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private AutoClosingMessageBox(string text, string caption, int timeout)
        {
            this._caption = caption;
            this._timeoutTimer = new System.Threading.Timer(
                this.OnTimerElapsed,
                null,
                timeout,
                System.Threading.Timeout.Infinite);
            using (this._timeoutTimer)
            {
                MessageBox.Show(text, caption);
            }
        }

        /// <summary>
        /// Show Auto Close Message Box
        /// </summary>
        /// <param name="text">Message</param>
        /// <param name="caption">Caption</param>
        /// <param name="timeout">Time</param>
        public static void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }

        private void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow("#32770", this._caption); // lpClassName is #32770 for MessageBox
            if (mbWnd != IntPtr.Zero)
            {
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }

            this._timeoutTimer.Dispose();
        }
    }
}
