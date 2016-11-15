using System;
using System . Collections . Generic;
using System . Globalization;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Data;

namespace MahoushoujoDesktop . Binding
{
    class ThicknessToDoubleConverter : IValueConverter
    {
        public object Convert ( object value , Type targetType , object parameter , CultureInfo culture )
        {
            var thickness = (Thickness) value;
            return ( thickness . Left + thickness . Top + thickness . Right + thickness . Bottom ) / 4;
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture )
        {
            var avg = (double) value;
            return new Thickness ( avg );
        }
    }
}
