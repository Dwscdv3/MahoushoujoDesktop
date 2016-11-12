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

namespace MahoushoujoDesktop . UI
{
    /// <summary>
    /// HoldButton.xaml 的交互逻辑
    /// </summary>
    public partial class HoldButton : Button
    {
        public Brush ProgressBrush
        {
            get { return (Brush) GetValue ( ProgressBrushProperty ); }
            set { SetValue ( ProgressBrushProperty , value ); }
        }
        public static readonly DependencyProperty ProgressBrushProperty =
            DependencyProperty . Register ( "ProgressBrush" , typeof ( Brush ) , typeof ( HoldButton ) , new PropertyMetadata ( Brushes.White ) );
        
        public HoldButton ()
        {
            InitializeComponent ();
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
}
