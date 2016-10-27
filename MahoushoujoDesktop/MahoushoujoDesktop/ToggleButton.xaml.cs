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
using System . Windows . Media . Imaging;
using System . Windows . Navigation;
using System . Windows . Shapes;

namespace MahoushoujoDesktop
{
    /// <summary>
    /// ToggleButton.xaml 的交互逻辑
    /// </summary>
    public partial class ToggleButton : System . Windows . Controls . Primitives . ToggleButton
    {
        static Color SkyBlue = Color . FromRgb ( 0 , 127 , 255 );
        static Color SkyBlueT = Color . FromArgb ( 0 , 0 , 127 , 255 );
        static Color Alpha1 = Color . FromArgb ( 1 , 255 , 255 , 255 );
        static Color QuarterWhite = Color . FromArgb ( 63 , 255 , 255 , 255 );
        static Color HalfWhite = Color . FromArgb ( 127 , 255 , 255 , 255 );
        static Color WhiteT = Color . FromArgb ( 0 , 255 , 255 , 255 );
        static LinearGradientBrush BrushSkyBlue = new LinearGradientBrush ( SkyBlueT , SkyBlue , 0 );
        static LinearGradientBrush BrushQuarterWhite = new LinearGradientBrush ( WhiteT , QuarterWhite , 0 );
        static LinearGradientBrush BrushHalfWhite = new LinearGradientBrush ( WhiteT , HalfWhite , 0 );
        static SolidColorBrush BrushDefault = new SolidColorBrush ( Alpha1 );

        public ToggleButton ()
        {
            InitializeComponent ();
        }

        private void ToggleButton_Loaded ( object sender , RoutedEventArgs e )
        {
            Background = IsChecked == true ? (Brush) BrushSkyBlue : BrushDefault;
        }

        private void ToggleButton_MouseEnter ( object sender , MouseEventArgs e )
        {
            Background = IsChecked == true
                ? new LinearGradientBrush ( SkyBlueT , ColorBlend ( SkyBlue , QuarterWhite ) , 0 )
                : BrushQuarterWhite;
        }

        private void ToggleButton_MouseLeave ( object sender , MouseEventArgs e )
        {
            Background = IsChecked == true ? (Brush) BrushSkyBlue : BrushDefault;
        }

        private void ToggleButton_MouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            Background = IsChecked == true
                ? new LinearGradientBrush ( SkyBlueT , ColorBlend ( SkyBlue , HalfWhite ) , 0 )
                : BrushHalfWhite;
        }

        private void ToggleButton_MouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
        {
            Background = IsChecked == !IsPressed
                ? new LinearGradientBrush ( SkyBlueT , ColorBlend ( SkyBlue , QuarterWhite ) , 0 )
                : BrushQuarterWhite;
        }

        public static Color ColorBlend ( Color c1 , Color c2 )
        {
            Color c = new Color ();
            c . R = (byte) Math . Sqrt ( c2 . R * c2 . A + ( byte . MaxValue - c2 . A ) * c1 . R );
            c . G = (byte) Math . Sqrt ( c2 . G * c2 . A + ( byte . MaxValue - c2 . A ) * c1 . G );
            c . B = (byte) Math . Sqrt ( c2 . B * c2 . A + ( byte . MaxValue - c2 . A ) * c1 . B );
            c . A = (byte) ( c1 . A + Math . Sqrt ( ( byte . MaxValue - c1 . A ) * c2 . A ) );
            return c;
        }
    }
}
