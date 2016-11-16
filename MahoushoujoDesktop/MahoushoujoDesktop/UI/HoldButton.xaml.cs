using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Data;
using System . Windows . Documents;
using System . Windows . Input;
using System . Windows . Media;
using System . Windows . Media . Animation;
using System . Windows . Media . Imaging;
using System . Windows . Navigation;
using System . Windows . Shapes;
using Dwscdv3 . WPF . UserControls;

namespace MahoushoujoDesktop . UI
{
    /// <summary>
    /// HoldButton.xaml 的交互逻辑
    /// </summary>
    public partial class HoldButton : Button
    {
        public event RoutedEventHandler SecondaryOperation;

        public Brush ProgressBrush
        {
            get { return (Brush) GetValue ( ProgressBrushProperty ); }
            set { SetValue ( ProgressBrushProperty , value ); }
        }
        public static readonly DependencyProperty ProgressBrushProperty =
            DependencyProperty . Register ( "ProgressBrush" , typeof ( Brush ) , typeof ( HoldButton ) , new PropertyMetadata ( Brushes . White ) );

        public MouseOperation MouseSecondaryOperation
        {
            get { return (MouseOperation) GetValue ( MouseSecondaryOperationProperty ); }
            set { SetValue ( MouseSecondaryOperationProperty , value ); }
        }
        public static readonly DependencyProperty MouseSecondaryOperationProperty =
            DependencyProperty . Register (
                "MouseSecondaryOperation" , typeof ( MouseOperation ) , typeof ( HoldButton ) ,
                new PropertyMetadata ( MouseOperation . RightButtonClick ) );

        public bool IsProgressing
        {
            get { return (bool) GetValue ( IsProgressingProperty ); }
            set { SetValue ( IsProgressingProperty , value ); }
        }
        public static readonly DependencyProperty IsProgressingProperty =
            DependencyProperty . Register ( "IsProgressing" , typeof ( bool ) , typeof ( HoldButton ) ,
                new PropertyMetadata ( false ) );
        
        private CircularProgress progress;
        private Storyboard progressStoryboard = null;

        public HoldButton ()
        {
            InitializeComponent ();
            progressStoryboard = Resources [ "ProgressStoryboard" ] as Storyboard;
        }

        private void startProgressing ()
        {
            progressStoryboard . Begin ();
            IsProgressing = true;
        }
        private void stopProgressing ()
        {
            progressStoryboard . Stop ();
            IsProgressing = false;
        }

        private void progressStoryboard_Completed ( object sender , EventArgs e )
        {
            IsProgressing = false;
            SecondaryOperation?.Invoke ( this , new RoutedEventArgs () );
            progressStoryboard . Stop ();
        }

        private void userControlHoldButton_MouseDown ( object sender , MouseButtonEventArgs e )
        {
            isTouchLastTime = false;
        }
        private void userControlHoldButton_MouseRightButtonDown ( object sender , MouseButtonEventArgs e )
        {
            if ( MouseSecondaryOperation == MouseOperation . RightButtonHold )
            {
                e . MouseDevice . Capture ( this );
                startProgressing ();
            }
            else
            {
                SecondaryOperation?.Invoke ( this , new RoutedEventArgs () );
            }
        }

        private void userControlHoldButton_MouseRightButtonUp ( object sender , MouseButtonEventArgs e )
        {
            ReleaseMouseCapture ();
        }
        private void userControlHoldButton_LostMouseCapture ( object sender , MouseEventArgs e )
        {
            stopProgressing ();
        }

        bool isTouchLastTime = false;
        DateTime touchDownTime = DateTime . MinValue;
        private void userControlHoldButton_TouchDown ( object sender , TouchEventArgs e )
        {
            isTouchLastTime = true;
            touchDownTime = DateTime . Now;
            e . TouchDevice . Capture ( this );
            startProgressing ();
        }
        private void userControlHoldButton_TouchUp ( object sender , TouchEventArgs e )
        {
            ReleaseTouchCapture ( e . TouchDevice );
        }
        private void userControlHoldButton_LostTouchCapture ( object sender , TouchEventArgs e )
        {
            stopProgressing ();
        }
        protected override void OnClick ()
        {
            if ( isTouchLastTime )
            {
                if ( DateTime . Now - touchDownTime < TimeSpan . FromMilliseconds ( 200 ) )
                {
                    base . OnClick ();
                }
            }
            else
            {
                base . OnClick ();
            }
        }

        public override void OnApplyTemplate ()
        {
            progress = GetTemplateChild ( "progress" ) as CircularProgress;
            Storyboard . SetTarget ( progressStoryboard . Children [ 0 ] , progress );

            base . OnApplyTemplate ();
        }

        /* TODO
         *   Property
         *     ProgressThickness
         *   Template
         *     Border: CircularProgress
         *   Event
         *     MouseRightButtonDown
         *       Mouse.Capture
         *     LostMouseCapture
         */
    }
    public enum MouseOperation
    {
        RightButtonClick,
        RightButtonHold
    }
}