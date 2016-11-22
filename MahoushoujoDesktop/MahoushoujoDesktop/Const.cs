using System;
using System . Collections . Generic;
using System . Windows;
using System . Windows . Forms;

namespace MahoushoujoDesktop
{
    public static class Const
    {
        public static string UrlApiBase
        {
            get
            {
                return "http://api.syouzyo.org/";
            }
        }
        public static string UrlApiV1
        {
            get
            {
                return UrlApiBase + "?";
            }
        }
        public static string UrlApiV2
        {
            get
            {
                return UrlApiBase + "v2/";
            }
        }

        public const double _mainWindowWidth = 250.0;
        public static double MainWindowWidth
        {
            get
            {
                return _mainWindowWidth;
            }
        }

        public static Thickness LabelHideMargin
        {
            get
            {
                return new Thickness ( 0 , 0 , MainWindowWidth , 0 );
            }
        }
        public static double ScreenRightBound
        {
            get
            {
                return Screen . PrimaryScreen . WorkingArea . Right;
            }
        }
        public static double ScreenRightBoundMinusMainWindowWidth
        {
            get
            {
                return ScreenRightBound - MainWindowWidth;
            }
        }

        public static string FileNameCurrentImage = "Current Image";
        public static string FileNameWindowHandle = "Window Handle";

        public static double NextIntervalsCount
        {
            get
            {
                return NextIntervals . Length - 1;
            }
        }
        public static readonly TimeSpanNode [] NextIntervals = new TimeSpanNode []
        {
            new TimeSpanNode { Description = "1 分钟" , TimeSpan = new TimeSpan ( 0 , 1 , 0 ) },
            new TimeSpanNode { Description = "5 分钟" , TimeSpan = new TimeSpan ( 0 , 5 , 0 ) },
            new TimeSpanNode { Description = "15 分钟" , TimeSpan = new TimeSpan ( 0 , 15 , 0 ) },
            new TimeSpanNode { Description = "1 小时" , TimeSpan = new TimeSpan ( 1 , 0 , 0 ) },
            new TimeSpanNode { Description = "4 小时" , TimeSpan = new TimeSpan ( 4 , 0 , 0 ) },
            new TimeSpanNode { Description = "12 小时" , TimeSpan = new TimeSpan ( 12 , 0 , 0 ) },
        };
    }

    public struct TimeSpanNode
    {
        public string Description;
        public TimeSpan TimeSpan;
    }
}
