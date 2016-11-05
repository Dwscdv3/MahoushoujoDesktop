using System;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Media;
using System . Windows . Shapes;

namespace Dwscdv3 . WPF . UserControls
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// 两年前的过于傻逼的代码，已重写，可能有bug （But it just works, huh?
    /// </summary>
    public partial class CircularProgress : UserControl
    {
        public double Value
        {
            get { return (double) GetValue ( ValueProperty ); }
            set { SetValue ( ValueProperty , value ); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty . Register ( "Value" , typeof ( double ) , typeof ( CircularProgress ) ,
                new PropertyMetadata ( 0.0 , new PropertyChangedCallback ( ValueChangedCallback ) ) );

        public double Minimum
        {
            get { return (double) GetValue ( MinimumProperty ); }
            set { SetValue ( MinimumProperty , value ); }
        }
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty . Register ( "Minimum" , typeof ( double ) , typeof ( CircularProgress ) ,
                new PropertyMetadata ( 0.0 , new PropertyChangedCallback ( ValueChangedCallback ) ) );

        public double Maximum
        {
            get { return (double) GetValue ( MaximumProperty ); }
            set { SetValue ( MaximumProperty , value ); }
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty . Register ( "Maximum" , typeof ( double ) , typeof ( CircularProgress ) ,
                new PropertyMetadata ( 100.0 , new PropertyChangedCallback ( ValueChangedCallback ) ) );

        public double StrokeThickness
        {
            get { return (double) GetValue ( StrokeThicknessProperty ); }
            set { SetValue ( StrokeThicknessProperty , value ); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty . Register ( "StrokeThickness" , typeof ( double ) , typeof ( CircularProgress ) ,
                new PropertyMetadata ( 1.0 , new PropertyChangedCallback ( ValueChangedCallback ) ) );
        

        public CircularProgress ()
        {
            InitializeComponent ();
        }

        static void ValueChangedCallback ( DependencyObject sender , DependencyPropertyChangedEventArgs e )
        {
            var progress = (CircularProgress) sender;
            progress . Render ();
        }

        public void Render ()
        {
            MainGrid . Children . Clear ();
            
            Path p = new Path ();
            p . StrokeThickness = StrokeThickness;
            p . Stroke = Foreground;
            p . Width = Width;
            p . Height = Height;

            PathGeometry g = new PathGeometry ();
            PathFigureCollection fc = new PathFigureCollection ();
            PathFigure f = new PathFigure ();
            f . StartPoint = new Point ( Width / 2 , StrokeThickness / 2 );
            PathSegmentCollection sc = new PathSegmentCollection ();

            double angle = ( Value - Minimum ) / ( Maximum - Minimum ) * 360;
            ArcSegment s = new ArcSegment ();
            s . Size = new Size ( Width / 2 - StrokeThickness / 2 , Height / 2 - StrokeThickness / 2 );
            s . SweepDirection = SweepDirection . Clockwise;
            s . IsLargeArc = angle >= 180 ? true : false;
            s . Point = GetPointOnCircle ( new Point ( Width / 2 , Height / 2 ) , s . Size . Width , angle );
            sc . Add ( s );
            f . Segments = sc;
            fc . Add ( f );
            g . Figures = fc;
            p . Data = g;

            MainGrid . Children . Add ( p );
        }
        
        Point GetPointOnCircle ( Point center , double r , double angle )
        {
            Point p = new Point ();
            p . X = center . X + Math . Sin ( angle * Math . PI / 180 ) * r;
            p . Y = center . Y - Math . Cos ( angle * Math . PI / 180 ) * r;
            return p;
        }
    }
}
