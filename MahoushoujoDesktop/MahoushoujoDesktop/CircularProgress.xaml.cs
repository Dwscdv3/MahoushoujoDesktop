using System;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Media;
using System . Windows . Shapes;

namespace Dwscdv3 . WPF . UserControls
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// TODO: 两年前的过于傻逼的代码，已部分重写（It just works, huh?
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
            //foreach (UIElement item in MainGrid.Children) {
            //	if (item is Path) {
            //		MainGrid.Children.Remove(item);
            //		break;
            //	}
            //}
            
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


        //public void Draw() {
        //	DrawingVisual dv = new DrawingVisual();
        //	DrawingContext dc = dv.RenderOpen();
        //	dc.DrawGeometry(null, new Pen(new SolidColorBrush(new Color() { R = 102, G = 204, B = 255 }), 3), GetGeometry());

        //}

        Point GetPointOnCircle ( Point center , double r , double angle )
        {
            Point p = new Point ();
            p . X = center . X + Math . Sin ( angle * Math . PI / 180 ) * r;
            p . Y = center . Y - Math . Cos ( angle * Math . PI / 180 ) * r;
            return p;
        }
        //private Geometry DrawArc(Point c1a, Point c1b, Point c2a, Point c2b, double c1r, double c2r, bool isLargeArc) {
        //	PathFigure pathFigure = new PathFigure { IsClosed = true };
        //	pathFigure.StartPoint = c1a;
        //	pathFigure.Segments.Add(new ArcSegment {
        //		  Point = c1b,
        //		  IsLargeArc = isLargeArc,
        //		  Size = new Size(c1r, c1r),
        //		  SweepDirection = SweepDirection.Clockwise
        //	  });
        //	pathFigure.Segments.Add(new LineSegment { Point = c2b });
        //	pathFigure.Segments.Add(new ArcSegment {
        //		 Point = c2a,
        //		 IsLargeArc = isLargeArc,
        //		 Size = new Size(c2r, c2r),
        //		 SweepDirection = SweepDirection.Counterclockwise
        //	 });
        //	PathGeometry pathGeometry = new PathGeometry();
        //	pathGeometry.Figures.Add(pathFigure);
        //	return pathGeometry;
        //}
        ////根据已保存的大小和文件总大小来计算下载进度百分比
        //private Geometry GetGeometry() {
        //	double percent = 0.42;
        //	//PercentString = string.Format("{0}%", Math.Round(percent * 100, 0));
        //	double angel = percent * 360.0;
        //	bool isLargeArc = angel > 180 ? true : false;
        //	//double angel = 45;
        //	double bigR = 16;
        //	double smallR = 13;
        //	Point centerPoint = new Point(100, 300);
        //	Point firstpoint = GetPointOnCircle(centerPoint, bigR, 0);
        //	Point secondpoint = GetPointOnCircle(centerPoint, bigR, angel);
        //	Point thirdpoint = GetPointOnCircle(centerPoint, smallR, 0);
        //	Point fourthpoint = GetPointOnCircle(centerPoint, smallR, angel);
        //	return DrawArc(firstpoint, secondpoint, thirdpoint, fourthpoint, bigR, smallR, isLargeArc);
        //}


        //public Visual DrawShape() {
        //	DrawingVisual dv = new DrawingVisual();
        //	DrawingContext dc = dv.RenderOpen();
        //	try {
        //		if (savedSize != fileSize) {
        //			dc.DrawEllipse(null, new Pen(Brushes.Gray, 3), vl.StartPoint, 13, 13);
        //			dc.DrawGeometry(vs.VisualBackgroundBrush, vs.VisualFramePen, GetGeometry());
        //			FormattedText formatWords = new FormattedText(PercentString, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(vs.WordsFont.Name), vs.WordsFont.Size, currentStyle.VisualBackgroundBrush);
        //			formatWords.SetFontWeight(FontWeights.Bold);
        //			Point startPoint = new Point(vl.StartPoint.X - formatWords.Width / 2, vl.StartPoint.Y - formatWords.Height / 2);
        //			dc.DrawText(formatWords, startPoint);
        //		} else {
        //			dc.DrawEllipse(null, new Pen(Brushes.Green, 3), vl.StartPoint, 16, 16);
        //			FormattedText formatWords = new FormattedText("Open", System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(vs.WordsFont.Name), vs.WordsFont.Size, Brushes.Red);
        //			formatWords.SetFontWeight(FontWeights.Bold);
        //			Point startPoint = new Point(vl.StartPoint.X - formatWords.Width / 2, vl.StartPoint.Y - formatWords.Height / 2);
        //			dc.DrawText(formatWords, startPoint);
        //		}
        //	} catch (Exception ex) {
        //		new SaveExceptionInfo().SaveLogAsTXTInfoex(ex.Message);
        //	} finally {
        //		dc.Close();
        //	}
        //	return dv;
        //}
    }
}
