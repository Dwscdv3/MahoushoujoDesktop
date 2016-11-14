using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Media;

namespace MahoushoujoDesktop . Util
{
    public static class ColorUtil
    {
        public static Color Random ()
        {
            var r = new Random ();
            return Color . FromRgb (
                (byte) r . Next ( 256 ) ,
                (byte) r . Next ( 256 ) ,
                (byte) r . Next ( 256 ) );
        }

        public static Color Blend ( Color c1 , Color c2 )
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
