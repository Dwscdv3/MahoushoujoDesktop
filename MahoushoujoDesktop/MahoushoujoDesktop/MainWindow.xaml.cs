using System;
using System . Collections . Generic;
using System . Diagnostics;
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
using MahoushoujoDesktop . Native;
using MahoushoujoDesktop . Util;
using static MahoushoujoDesktop . Const;
using static MahoushoujoDesktop . Mahoushoujo;
using static MahoushoujoDesktop . Native . Managed;
using static MahoushoujoDesktop . Native . User32;
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
        Storyboard slideOutStoryboard = null,
                   slideInStoryboard = null,
                   progressCircleExitStoryboard = null;

        public MainWindow ()
        {
            InitializeComponent ();
            setWindowPosition ();

            slideOutStoryboard = (Storyboard) Resources [ "SlideOutStoryboard" ];
            slideInStoryboard = (Storyboard) Resources [ "SlideInStoryboard" ];
            progressCircleExitStoryboard = (Storyboard) Resources [ "ProgressCircleExitStoryboard" ];

            keyboardEvent . KeyDown += KeyboardEvent_KeyDown;
        }

        IntPtr hWnd;
        private async void window_Loaded ( object sender , RoutedEventArgs e )
        {
            hWnd = new WindowInteropHelper ( this ) . Handle;
            //SetWindowLong ( hWnd , (int) WindowLongFlags . GWL_EXSTYLE ,
            //    GetWindowLong ( hWnd , (int) WindowLongFlags . GWL_EXSTYLE ) |
            //    (int) ExtendedWindowStyle . NoActivate );
            //SetWindowPos ( hWnd , HWND_BOTTOM , 0 , 0 , 0 , 0 ,
            //    SetWindowPosFlags . SWP_NOMOVE |
            //    SetWindowPosFlags . SWP_NOSIZE |
            //    SetWindowPosFlags . SWP_NOACTIVATE );

            #region 创建窗口句柄文件
            if ( File . Exists ( FileNameWindowHandle ) )
            {
                File . Delete ( FileNameWindowHandle );
            }
            FileInfo file = new FileInfo ( FileNameWindowHandle );
            var stream = file . CreateText ();
            stream . Write ( hWnd . ToString () );
            stream . Flush ();
            stream . Close ();
            file . Attributes = file . Attributes | FileAttributes . Hidden;
            #endregion

            progressCircleExit . StrokeThickness = 5.0;

            Mahoushoujo . Init ();
            buttonMainSwitch . IsChecked = Default . MainSwitch;
            sliderInterval . Value = Default . NextIntervalSliderValue;
            textInterval . Text = NextIntervals [ (int) sliderInterval . Value ] . Description;
            SetTimerIntervalBySliderValue ();

            CurrentTab = 0;

            if ( !string . IsNullOrEmpty ( Default . UserAccessToken ) )
            {
                var user = await User . GetUserInfoByToken ( Default . UserAccessToken );
                if ( user . Info . uid > 0 )
                {
                    LogInUser = user;
                }
            }
        }

        private void window_Activated ( object sender , EventArgs e )
        {
            //SetWindowPos ( hWnd , HWND_BOTTOM , 0 , 0 , 0 , 0 ,
            //    SetWindowPosFlags . SWP_NOMOVE |
            //    SetWindowPosFlags . SWP_NOSIZE |
            //    SetWindowPosFlags . SWP_NOACTIVATE );
        }

        private void window_SizeChanged ( object sender , EventArgs e )
        {
            //if ( WindowState == WindowState . Minimized )
            //{
            //    WindowState = WindowState . Normal;
            //}
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

        private void sliderInterval_LostMouseCapture ( object sender , MouseEventArgs e )
        {
            SetTimerIntervalBySliderValue ();
        }

        private void SetTimerIntervalBySliderValue ()
        {
            TimerInterval = NextIntervals [ (int) sliderInterval . Value ] . TimeSpan;
            Default . NextIntervalSliderValue = sliderInterval . Value;
        }

        private void sliderInterval_ValueChanged ( object sender , RoutedPropertyChangedEventArgs<double> e )
        {
            if ( textInterval != null )
            {
                textInterval . Text = NextIntervals [ (int) e . NewValue ] . Description;
            }
        }

        private void ProgressCircleExit_Completed ( object sender , EventArgs e )
        {
            Close ();
        }



        public bool IsLoggingIn
        {
            get { return (bool) GetValue ( IsLoggingInProperty ); }
            set { SetValue ( IsLoggingInProperty , value ); }
        }
        public static readonly DependencyProperty IsLoggingInProperty =
            DependencyProperty . Register ( "IsLoggingIn" , typeof ( bool ) , typeof ( MainWindow ) , new PropertyMetadata ( false ) );


        private async void buttonLogin_Click ( object sender , RoutedEventArgs e )
        {
            if ( !IsLoggingIn )
            {
                User user = null;
                try
                {
                    IsLoggingIn = true;
                    user = await User . LogIn ( textBoxUsername . Text , passwordBox . Password );
                }
                catch ( Exception ex )
                {
                    Debug . WriteLine ( ex . Message );
                }
                finally
                {
                    IsLoggingIn = false;
                }

                if ( user != null )
                {
                    LogInUser = user;
                    passwordBox . Password = "";
                }
                else
                {
                    // TODO: Log in fail
                }
            }
        }

        public void SetUserPanel ()
        {
            if ( !string . IsNullOrEmpty ( LogInUser?.Info?.name ) )
            {
                SetEllipseUserAvatar ();
                textBlockUserName . Text = LogInUser . Info . name;
                stackPanelLogin . Visibility = Visibility . Collapsed;
                stackPanelMe . Visibility = Visibility . Visible;
            }
        }

        private void SetEllipseUserAvatar ()
        {
            var avatarUrl = LogInUser?.Info?.avatar;
            if ( !string . IsNullOrEmpty ( avatarUrl ) )
            {
                if ( avatarUrl [ 0 ] == '/' )
                {
                    avatarUrl = avatarUrl . Insert ( 0 , "http:" );
                }
                ( (ImageBrush) ellipseUserAvatar . Fill ) . ImageSource = new BitmapImage ( new Uri ( avatarUrl ) );
            }
            else
            {
                ( (ImageBrush) ellipseUserAvatar . Fill ) . ImageSource = null;
            }
        }

        private void buttonSignup_Click ( object sender , RoutedEventArgs e )
        {
            Process . Start ( "http://api.mouto.org/reg.html#!redirect=http%3A%2F%2Fsyouzyo.org%2F" );
        }

        private void tabHeader_Click ( object sender , RoutedEventArgs e )
        {
            var element = sender as FrameworkElement;
            var parent = element . Parent as FrameworkElement;
            var grandParent = parent . Parent as Panel;
            int index = -1;
            foreach ( var item in grandParent . Children )
            {
                index++;
                if ( parent == item )
                {
                    CurrentTab = index;
                    break;
                }
            }
        }

        private void passwordBox_KeyDown ( object sender , KeyEventArgs e )
        {
            if ( e . Key == Key . Enter )
            {
                buttonLogin . Focus ();
                buttonLogin_Click ( this , new RoutedEventArgs () );
            }
        }

        const int TabSwitchAnimationDurationInMilliseconds = 150;
        const int TabSwitchAnimationMovingDistance = 25;
        int _currentTab = -1;
        public int CurrentTab
        {
            get
            {
                return _currentTab;
            }
            set
            {
                UIElement oldTab = null;
                if ( _currentTab >= 0 )
                {
                    oldTab = gridTab . Children [ _currentTab ];
                }
                var newTab = gridTab . Children [ value ];

                Grid . SetColumn ( ellipseSelectedTab , value + 1 );

                if ( value > _currentTab )
                {
                    #region Storyboard - Tab 向左滑动
                    Storyboard sbOut = new Storyboard ();

                    if ( oldTab != null )
                    {
                        ThicknessAnimation marginOut = new ThicknessAnimation (
                            new Thickness ( 0 ) ,
                            new Thickness ( -TabSwitchAnimationMovingDistance , 0 , TabSwitchAnimationMovingDistance , 0 ) ,
                            new Duration ( TimeSpan . FromMilliseconds ( TabSwitchAnimationDurationInMilliseconds ) ) );
                        Storyboard . SetTarget ( marginOut , oldTab );
                        Storyboard . SetTargetProperty ( marginOut , new PropertyPath ( "Margin" ) );
                        sbOut . Children . Add ( marginOut );

                        DoubleAnimation opacityOut = new DoubleAnimation (
                            1 , 0 , new Duration ( TimeSpan . FromMilliseconds ( TabSwitchAnimationDurationInMilliseconds ) ) );
                        Storyboard . SetTarget ( opacityOut , oldTab );
                        Storyboard . SetTargetProperty ( opacityOut , new PropertyPath ( "Opacity" ) );
                        sbOut . Children . Add ( opacityOut );
                    }

                    if ( newTab != null )
                    {
                        if ( oldTab != null )
                        {
                            sbOut . Completed += ( sender , e ) =>
                            {
                                Storyboard sbIn = new Storyboard ();

                                ThicknessAnimation marginIn = new ThicknessAnimation (
                                    new Thickness ( TabSwitchAnimationMovingDistance , 0 , -TabSwitchAnimationMovingDistance , 0 ) ,
                                    new Thickness ( 0 ) ,
                                    new Duration ( TimeSpan . FromMilliseconds ( TabSwitchAnimationDurationInMilliseconds ) )
                                );
                                Storyboard . SetTarget ( marginIn , newTab );
                                Storyboard . SetTargetProperty ( marginIn , new PropertyPath ( "Margin" ) );
                                sbIn . Children . Add ( marginIn );

                                DoubleAnimation opacityIn = new DoubleAnimation (
                                    0 , 1 , new Duration ( TimeSpan . FromMilliseconds ( TabSwitchAnimationDurationInMilliseconds ) ) );
                                Storyboard . SetTarget ( opacityIn , newTab );
                                Storyboard . SetTargetProperty ( opacityIn , new PropertyPath ( "Opacity" ) );
                                sbIn . Children . Add ( opacityIn );

                                sbIn . Begin ();
                            };
                        }
                        else
                        {
                            Storyboard sbIn = new Storyboard ();

                            ThicknessAnimation marginIn = new ThicknessAnimation (
                                new Thickness ( TabSwitchAnimationMovingDistance , 0 , -TabSwitchAnimationMovingDistance , 0 ) ,
                                new Thickness ( 0 ) ,
                                new Duration ( TimeSpan . FromMilliseconds ( TabSwitchAnimationDurationInMilliseconds ) )
                            );
                            Storyboard . SetTarget ( marginIn , newTab );
                            Storyboard . SetTargetProperty ( marginIn , new PropertyPath ( "Margin" ) );
                            sbIn . Children . Add ( marginIn );

                            DoubleAnimation opacityIn = new DoubleAnimation (
                                0 , 1 , new Duration ( TimeSpan . FromMilliseconds ( TabSwitchAnimationDurationInMilliseconds ) ) );
                            Storyboard . SetTarget ( opacityIn , newTab );
                            Storyboard . SetTargetProperty ( opacityIn , new PropertyPath ( "Opacity" ) );
                            sbIn . Children . Add ( opacityIn );

                            sbIn . Begin ();
                        }
                    }

                    sbOut . Begin ();
                    #endregion
                }
                else if ( value < _currentTab )
                {
                    #region Storyboard - Tab 向右滑动
                    Storyboard sbOut = new Storyboard ();

                    if ( oldTab != null )
                    {
                        ThicknessAnimation marginOut = new ThicknessAnimation (
                            new Thickness ( 0 ) ,
                            new Thickness ( TabSwitchAnimationMovingDistance , 0 , -TabSwitchAnimationMovingDistance , 0 ) ,
                            new Duration ( TimeSpan . FromMilliseconds ( TabSwitchAnimationDurationInMilliseconds ) ) );
                        Storyboard . SetTarget ( marginOut , oldTab );
                        Storyboard . SetTargetProperty ( marginOut , new PropertyPath ( "Margin" ) );
                        sbOut . Children . Add ( marginOut );

                        DoubleAnimation opacityOut = new DoubleAnimation (
                            1 , 0 , new Duration ( TimeSpan . FromMilliseconds ( TabSwitchAnimationDurationInMilliseconds ) ) );
                        Storyboard . SetTarget ( opacityOut , oldTab );
                        Storyboard . SetTargetProperty ( opacityOut , new PropertyPath ( "Opacity" ) );
                        sbOut . Children . Add ( opacityOut );
                    }

                    if ( newTab != null )
                    {
                        if ( oldTab != null )
                        {
                            sbOut . Completed += ( sender , e ) =>
                            {
                                Storyboard sbIn = new Storyboard ();

                                ThicknessAnimation marginIn = new ThicknessAnimation (
                                    new Thickness ( -TabSwitchAnimationMovingDistance , 0 , TabSwitchAnimationMovingDistance , 0 ) ,
                                    new Thickness ( 0 ) ,
                                    new Duration ( TimeSpan . FromMilliseconds ( TabSwitchAnimationDurationInMilliseconds ) )
                                );
                                Storyboard . SetTarget ( marginIn , newTab );
                                Storyboard . SetTargetProperty ( marginIn , new PropertyPath ( "Margin" ) );
                                sbIn . Children . Add ( marginIn );

                                DoubleAnimation opacityIn = new DoubleAnimation (
                                    0 , 1 , new Duration ( TimeSpan . FromMilliseconds ( TabSwitchAnimationDurationInMilliseconds ) ) );
                                Storyboard . SetTarget ( opacityIn , newTab );
                                Storyboard . SetTargetProperty ( opacityIn , new PropertyPath ( "Opacity" ) );
                                sbIn . Children . Add ( opacityIn );

                                sbIn . Begin ();
                            };
                        }
                        else
                        {
                            Storyboard sbIn = new Storyboard ();

                            ThicknessAnimation marginIn = new ThicknessAnimation (
                                new Thickness ( -TabSwitchAnimationMovingDistance , 0 , TabSwitchAnimationMovingDistance , 0 ) ,
                                new Thickness ( 0 ) ,
                                new Duration ( TimeSpan . FromMilliseconds ( TabSwitchAnimationDurationInMilliseconds ) )
                            );
                            Storyboard . SetTarget ( marginIn , newTab );
                            Storyboard . SetTargetProperty ( marginIn , new PropertyPath ( "Margin" ) );
                            sbIn . Children . Add ( marginIn );

                            DoubleAnimation opacityIn = new DoubleAnimation (
                                0 , 1 , new Duration ( TimeSpan . FromMilliseconds ( TabSwitchAnimationDurationInMilliseconds ) ) );
                            Storyboard . SetTarget ( opacityIn , newTab );
                            Storyboard . SetTargetProperty ( opacityIn , new PropertyPath ( "Opacity" ) );
                            sbIn . Children . Add ( opacityIn );

                            sbIn . Begin ();
                        }
                    }

                    sbOut . Begin ();
                    #endregion
                }

                _currentTab = value;
            }
        }

        private void holdButton_SecondaryOperation ( object sender , RoutedEventArgs e )
        {
            holdButton . Foreground = new SolidColorBrush ( ColorUtil . Random () );
        }

        private void buttonLogOut_Click ( object sender , RoutedEventArgs e )
        {
            LogOut ();
        }

        private void LogOut ()
        {
            LogInUser = null;
            SetEllipseUserAvatar ();
            stackPanelMe . Visibility = Visibility . Collapsed;
            stackPanelLogin . Visibility = Visibility . Visible;
        }
    }
}
