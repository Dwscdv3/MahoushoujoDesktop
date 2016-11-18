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
                new PropertyMetadata (
                    1.0 ,
                    new PropertyChangedCallback ( ValueChangedCallback ) ,
                    ( sender , e ) =>
                    {
                        var progress = (Control) sender;
                        var oldValue = (double) e;
                        if ( progress . IsLoaded )
                        {
                            var sideLength = progress . ActualWidth > progress . ActualHeight
                                           ? progress . ActualHeight
                                           : progress . ActualWidth;

                            if ( oldValue > sideLength / 2 )
                            {
                                return sideLength / 2;
                            }
                        }
                        if ( oldValue <= 0 )
                        {
                            return 1.0;
                        }
                        return e;
                    }
                ) );

        public CircularProgress ()
        {
            InitializeComponent ();
        }

        private void UserControl_Loaded ( object sender , RoutedEventArgs e )
        {
            Render ();
        }

        static void ValueChangedCallback ( DependencyObject sender , DependencyPropertyChangedEventArgs e )
        {
            var progress = (CircularProgress) sender;
            progress . Render ();
        }

        protected override void OnChildDesiredSizeChanged ( UIElement child )
        {
            base . OnChildDesiredSizeChanged ( child );
        }

        public void Render ()
        {
            if ( IsLoaded )
            {
                MainGrid . Children . Clear ();

                if ( ActualWidth > 0 && ActualHeight > 0 )
                {

                    var sideLength = ActualWidth > ActualHeight ? ActualHeight : ActualWidth;

                    Path p = new Path ();
                    p . StrokeThickness = StrokeThickness;
                    p . Stroke = Foreground;
                    p . Width = p . Height = sideLength;

                    PathGeometry g = new PathGeometry ();
                    PathFigureCollection fc = new PathFigureCollection ();
                    PathFigure f = new PathFigure ();
                    f . StartPoint = new Point ( sideLength / 2 , StrokeThickness / 2 );
                    PathSegmentCollection sc = new PathSegmentCollection ();

                    double angle = ( Value - Minimum ) / ( Maximum - Minimum ) * 360;
                    ArcSegment s = new ArcSegment ();
                    s . Size = new Size ( sideLength / 2 - StrokeThickness / 2 , sideLength / 2 - StrokeThickness / 2 );
                    s . SweepDirection = SweepDirection . Clockwise;
                    if ( angle >= 360 )
                    {
                        s . IsLargeArc = true;
                        s . Point = GetPointOnCircle ( new Point ( sideLength / 2 , sideLength / 2 ) , s . Size . Width , 359.99 );
                    }
                    else if ( angle > 0 )
                    {
                        s . IsLargeArc = angle >= 180 ? true : false;
                        s . Point = GetPointOnCircle ( new Point ( sideLength / 2 , sideLength / 2 ) , s . Size . Width , angle );
                    }
                    else
                    {
                        s . Point = GetPointOnCircle ( new Point ( sideLength / 2 , sideLength / 2 ) , s . Size . Width , 0 );
                    }
                    sc . Add ( s );
                    f . Segments = sc;
                    fc . Add ( f );
                    g . Figures = fc;
                    p . Data = g;

                    MainGrid . Children . Add ( p );
                }
            }
        }

        Point GetPointOnCircle ( Point center , double r , double angle )
        {
            Point p = new Point ();
            p . X = center . X + Math . Sin ( angle * Math . PI / 180 ) * r;
            p . Y = center . Y - Math . Cos ( angle * Math . PI / 180 ) * r;
            return p;
        }

        protected override void OnPropertyChanged ( DependencyPropertyChangedEventArgs e )
        {
            base . OnPropertyChanged ( e );
            if ( IsLoaded )
            {
                Render ();
            }
        }
    }
}
