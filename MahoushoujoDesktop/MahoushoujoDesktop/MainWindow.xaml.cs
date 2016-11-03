using System;
using System . Collections . Generic;
using System . IO;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Documents;
using System . Windows . Input;
using System . Windows . Interop;
using System . Windows . Media;
using System . Windows . Media . Animation;
using System . Windows . Media . Imaging;
using Gma . System . MouseKeyHook;
using static MahoushoujoDesktop . Const;
using static MahoushoujoDesktop . Mahoushoujo;
using static MahoushoujoDesktop . Properties . Settings;
using WM = MahoushoujoDesktop . Native . WindowMessage;

namespace MahoushoujoDesktop
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        IKeyboardEvents keyboardEvent = Hook . GlobalEvents ();
        Storyboard slideOutStoryboard = null, slideInStoryboard = null, progressCircleExitStoryboard;

        public MainWindow ()
        {
            InitializeComponent ();
            setWindowPosition ();

            slideOutStoryboard = (Storyboard) Resources [ "SlideOutStoryboard" ];
            slideInStoryboard = (Storyboard) Resources [ "SlideInStoryboard" ];
            progressCircleExitStoryboard = (Storyboard) Resources [ "ProgressCircleExitStoryboard" ];

            keyboardEvent . KeyDown += KeyboardEvent_KeyDown;
        }

        private void window_Loaded ( object sender , RoutedEventArgs e )
        {
            #region 创建窗口句柄文件
            if ( File . Exists ( FileNameWindowHandle ) )
            {
                File . Delete ( FileNameWindowHandle );
            }
            FileInfo file = new FileInfo ( FileNameWindowHandle );
            var stream = file . CreateText ();
            stream . Write ( new WindowInteropHelper ( this ) . Handle . ToString () );
            stream . Flush ();
            stream . Close ();
            file . Attributes = file . Attributes | FileAttributes . Hidden;
            #endregion

            progressCircleExit . StrokeThickness = 5.0;

            Mahoushoujo . Init ();
            buttonMainSwitch . IsChecked = Default . MainSwitch;
        }

        // https://social.msdn.microsoft.com/Forums/zh-CN/6a9bca0a-c1e3-48b2-b1ba-716233e8a080
        // Template by "Jiwen Wang"
        protected override void OnSourceInitialized ( EventArgs e )
        {
            base . OnSourceInitialized ( e );

            IntPtr hwnd = new WindowInteropHelper ( this ) . Handle;
            HwndSource source = HwndSource . FromHwnd ( hwnd );
            source . AddHook ( new HwndSourceHook ( WindowProc ) );
        }
        IntPtr WindowProc ( IntPtr hwnd , int msg , IntPtr wParam , IntPtr lParam , ref bool handled )
        {
            switch ( msg )
            {
            case WM . DisplayChange:
                Task . Delay ( 1000 ) . ContinueWith ( t =>
                {
                    setWindowPosition ();
                } );
                break;
            case WM . Custom_ShowWindow:
                if ( !IsVisible )
                {
                    Show ();
                }
                break;
            }
            return IntPtr . Zero;
        }

        public new void Show ()
        {
            base . Show ();
            slideInStoryboard . Begin ();
            setWindowPosition ();
        }
        public new void Hide ()
        {
            slideOutStoryboard . Begin ();
        }

        private void KeyboardEvent_KeyDown ( object sender , System . Windows . Forms . KeyEventArgs e )
        {
            if ( e . Control && e . Alt && e . KeyCode == System . Windows . Forms . Keys . W )
            {
                if ( IsVisible )
                {
                    Hide ();
                }
                else
                {
                    Show ();
                }
            }
        }

        void setWindowPosition ()
        {
            var workingArea = System . Windows . Forms . Screen . PrimaryScreen . WorkingArea;
            Left = workingArea . Right - MainWindowWidth;
            Top = workingArea . Top;
            Width = MainWindowWidth;
            Height = workingArea . Height;
        }

        private void SlideOut_Completed ( object sender , EventArgs e )
        {
            base . Hide ();
        }

        private void buttonMainSwitch_Click ( object sender , RoutedEventArgs e )
        {
            IsTimerEnabled = !IsTimerEnabled;
        }
        private void buttonPrev_Click ( object sender , RoutedEventArgs e )
        {
            ResetTimerProgress ();
            Previous ();
        }
        private void buttonNext_Click ( object sender , RoutedEventArgs e )
        {
            ResetTimerProgress ();
            Next ();
        }

        private void rectHide_MouseRightButtonDown ( object sender , MouseButtonEventArgs e )
        {
            progressCircleExitStoryboard . Begin ();
        }
        private void rectHide_MouseRightButtonUp ( object sender , MouseButtonEventArgs e )
        {
            progressCircleExitStoryboard . Stop ();
        }
        private void rectHide_MouseLeave ( object sender , MouseEventArgs e )
        {
            progressCircleExitStoryboard . Stop ();
        }
        private void ProgressCircleExit_Completed ( object sender , EventArgs e )
        {
            Close ();
        }

    }
}
