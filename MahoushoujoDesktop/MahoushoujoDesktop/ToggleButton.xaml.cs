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
        static Color ColorChecked = Color . FromRgb ( 255 , 127 , 191 );
        static Color ColorCheckedT = Color . FromArgb ( 0 , 255 , 127 , 191 );
        static Color Alpha1 = Color . FromArgb ( 1 , 255 , 255 , 255 );
        static Color BlendColorMouseOver = Color . FromArgb ( 63 , 255 , 255 , 255 );
        static Color BlendColorMouseDown = Color . FromArgb ( 127 , 255 , 255 , 255 );
        static Color WhiteT = Color . FromArgb ( 0 , 255 , 255 , 255 );
        static LinearGradientBrush BrushSkyBlue = new LinearGradientBrush ( SkyBlueT , SkyBlue , 0 );
        static LinearGradientBrush BrushPink = new LinearGradientBrush ( ColorCheckedT , ColorChecked , 0 );
        static LinearGradientBrush BrushQuarterWhite = new LinearGradientBrush ( WhiteT , BlendColorMouseOver , 0 );
        static LinearGradientBrush BrushHalfWhite = new LinearGradientBrush ( WhiteT , BlendColorMouseDown , 0 );
        static SolidColorBrush BrushDefault = new SolidColorBrush ( Alpha1 );

        public ToggleButton ()
        {
            InitializeComponent ();
        }

        private void ToggleButton_Loaded ( object sender , RoutedEventArgs e )
        {
            Background = IsChecked == true ? (Brush) BrushPink : BrushDefault;
        }

        private void ToggleButton_MouseEnter ( object sender , MouseEventArgs e )
        {
            Background = IsChecked == true
                ? new LinearGradientBrush ( ColorCheckedT , ColorBlend ( ColorChecked , BlendColorMouseOver ) , 0 )
                : BrushQuarterWhite;
        }

        private void ToggleButton_MouseLeave ( object sender , MouseEventArgs e )
        {
            Background = IsChecked == true ? (Brush) BrushPink : BrushDefault;
        }

        private void ToggleButton_MouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            Background = IsChecked == true
                ? new LinearGradientBrush ( ColorCheckedT , ColorBlend ( ColorChecked , BlendColorMouseDown ) , 0 )
                : BrushHalfWhite;
        }

        private void ToggleButton_MouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
        {
            Background = IsChecked == !IsPressed
                ? new LinearGradientBrush ( ColorCheckedT , ColorBlend ( ColorChecked , BlendColorMouseOver ) , 0 )
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
