using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Documents;
using System . Windows . Forms;
using System . Windows . Input;
using System . Windows . Interop;
using System . Windows . Media;
using System . Windows . Media . Imaging;
using static MahoushoujoDesktop . Const;
using WM = MahoushoujoDesktop . Native . WindowMessage;

namespace MahoushoujoDesktop
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow ()
        {
            InitializeComponent ();
            setWindowPosition ();
        }

        void setWindowPosition ()
        {
            var workingArea = Screen . PrimaryScreen . WorkingArea;
            this . Left = workingArea . Right - MainWindowWidth;
            this . Top = workingArea . Top;
            this . Width = MainWindowWidth;
            this . Height = workingArea . Height;
        }

        // https://social.msdn.microsoft.com/Forums/zh-CN/6a9bca0a-c1e3-48b2-b1ba-716233e8a080 by Jiwen Wang
        IntPtr WindowProc ( IntPtr hwnd , int msg , IntPtr wParam , IntPtr lParam , ref bool handled )
        {
            switch ( msg )
            {
            case WM . DisplayChange:
                setWindowPosition ();
                break;
            }
            return IntPtr . Zero;
        }
        protected override void OnSourceInitialized ( EventArgs e )
        {
            base . OnSourceInitialized ( e );

            IntPtr hwnd = new WindowInteropHelper ( this ) . Handle;
            HwndSource source = HwndSource . FromHwnd ( hwnd );
            source . AddHook ( new HwndSourceHook ( WindowProc ) );
        }

        private void buttonHide_MouseEnter ( object sender , System . Windows . Input . MouseEventArgs e )
        {

        }

        private void HideOut_Completed ( object sender , EventArgs e )
        {
            this . Close (); // FIXIT: 未部署代码
        }
    }
}
