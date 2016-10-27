using System;
using System . Windows;
using System . Windows . Forms;

namespace MahoushoujoDesktop
{
    public static class Const
    {
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
                return Screen . PrimaryScreen . Bounds . Right;
            }
        }
    }
}
