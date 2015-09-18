using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultimediaKeysMapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public KeyboardHotKeys keyHandler;

        public MainWindow()
        {
            InitializeComponent();

            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();

            System.IO.Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/Icons/App.ico")).Stream;
            ni.Icon = new System.Drawing.Icon(iconStream);

            ni.Visible = true;
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    Show();
                    WindowState = WindowState.Normal;
                };

            // and hide
            Hide();

            // register to run at startup
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rkApp.GetValue("MachMultimediaKeysMapper") == null)
            {
                rkApp.SetValue("MachMultimediaKeysMapper", System.Reflection.Assembly.GetExecutingAssembly().Location);
            }

        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();

            base.OnStateChanged(e);
        }

        // https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var _host = new System.Windows.Interop.WindowInteropHelper(this);

            keyHandler = new KeyboardHotKeys(this, 202);
            keyHandler.MyKeyPressed += new EventHandler(k_MyKeyPressed);
            bool registration = keyHandler.RegisterHotKey(MainKey.WinKey, 0x71); //delete
            if (!registration)
            {
                MessageBox.Show("Nepovedlo se asociovat klávesovou zkratku pro pauzu.");
            }


            keyHandler = new KeyboardHotKeys(this, 203);
            keyHandler.MyKeyPressed += new EventHandler(k_MyKeyPressed);
            registration = keyHandler.RegisterHotKey(MainKey.WinKey, 0x72); // end
            if (!registration)
            {
                MessageBox.Show("Nepovedlo se asociovat klávesovou zkratku pro rychlé odesílání.");
            }

            keyHandler = new KeyboardHotKeys(this, 204);
            keyHandler.MyKeyPressed += new EventHandler(k_MyKeyPressed);
            registration = keyHandler.RegisterHotKey(MainKey.WinKey, 0x73); // page down
            if (!registration)
            {
                MessageBox.Show("Nepovedlo se asociovat klávesovou zkratku pro rychlé odesílání.");
            }
        }

        void k_MyKeyPressed(object sender, EventArgs e)
        {
            if (sender is KeyboardHotKeys)
            {
                KeyboardHotKeys handler = sender as KeyboardHotKeys;

                switch (handler.Hid)
                {
                    case 202:
                        keyHandler.SendPreviousTrack();
                        break;
                    case 203:
                        keyHandler.SendPause();
                        break;
                    case 204:
                        keyHandler.SendNextTrack();
                        break;
                }
            }
        }
    }
}
