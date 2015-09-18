using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace MultimediaKeysMapper
{
    public class KeyboardHotKeys : IDisposable
    {
        public event EventHandler MyKeyPressed;

        public const int WM_HOTKEY = 0x0312;
        //public const int VIRTUALKEYCODE = 0x14; //caps
        //public const int VIRTUALKEYCODE = 0x57; //W (http://msdn.microsoft.com/en-us/library/dd375731%28v=VS.85%29.aspx)
        public const int VIRTUALKEYCODE = 0x53; //S

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc); //(http://msdn.microsoft.com/en-us/library/ms646309%28VS.85%29.aspx)

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int WM_APPCOMMAND = 0x319;
        private const int APPCOMMAND_MEDIA_PLAY_PAUSE = 0xE0000;
        private const int APPCOMMAND_MEDIA_NEXTTRACK = 11 << 16;
        private const int APPCOMMAND_MEDIA_PREVIOUSTRACK = 12 << 16;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        internal void SendPause()
        {
            SendMessageW(_host.Handle, WM_APPCOMMAND, _host.Handle, (IntPtr)APPCOMMAND_MEDIA_PLAY_PAUSE);
        }

        public void SendNextTrack()
        {
            //int CommandID = (int)11 << 16;

            SendMessageW(_host.Handle, WM_APPCOMMAND, _host.Handle, (IntPtr)APPCOMMAND_MEDIA_NEXTTRACK);
        }

        public void SendPreviousTrack()
        {
            //int CommandID = (int)12 << 16;

            SendMessageW(_host.Handle, WM_APPCOMMAND, _host.Handle, (IntPtr)APPCOMMAND_MEDIA_PREVIOUSTRACK);
        }

        private readonly Window _mainWindow;
        WindowInteropHelper _host;

        private int hid;

        public int Hid
        {
            get { return hid; }
        }

        public static bool IsCorrespondingKeyCode(MainKey key, int code)
        {
            Enum keyEnum = (Enum)key;
            FieldInfo fieldInfo = keyEnum.GetType().GetField(keyEnum.ToString());

            object[] attribArray = fieldInfo.GetCustomAttributes(false);

            foreach (object attr in attribArray)
            {
                if (attr is CorrespondingKeyCodeAttribute)
                {
                    var corCode = attr as CorrespondingKeyCodeAttribute;

                    if (corCode.Code == code)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public KeyboardHotKeys(Window mainWindow)
        {
            hid = GetType().GetHashCode();
            _mainWindow = mainWindow;
            _host = new WindowInteropHelper(_mainWindow);

            ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
        }

        public KeyboardHotKeys(Window mainWindow, int id)
        {
            hid = id;
            _mainWindow = mainWindow;
            _host = new WindowInteropHelper(_mainWindow);

            ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
        }

        void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            if (msg.message == WM_HOTKEY)
            {
                if (msg.wParam == (IntPtr)hid)
                {
                    //Handle hot key kere
                    if (MyKeyPressed != null)
                        MyKeyPressed(this, EventArgs.Empty);
                }
            }
        }

        public bool RegisterHotKey()
        {
            return RegisterHotKey(MainKey.WinKey, VIRTUALKEYCODE);
        }

        public bool RegisterHotKey(MainKey main, int second)
        {
            return SetupHotKey(_host.Handle, (int)main, second);
        }

        public void UnregisterHotKey()
        {
            bool status = UnregisterHotKey(_host.Handle, hid);
            if (!status)
            {
#if DEBUG
                MessageBox.Show(Marshal.GetLastWin32Error().ToString());
#endif
            }
        }

        private bool SetupHotKey(IntPtr handle, int main, int second)
        {
            bool status = RegisterHotKey(handle, hid, main, second);

            return status;
        }

        public void Dispose()
        {
            UnregisterHotKey();
        }
    }
}
