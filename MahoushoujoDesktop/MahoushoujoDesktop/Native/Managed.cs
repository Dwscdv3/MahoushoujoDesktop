using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using static MahoushoujoDesktop . Native . User32;

namespace MahoushoujoDesktop . Native
{
    public static class Managed
    {
        public static bool SetWallpaperLegacy ( string path )
        {
            return SystemParametersInfo ( SPI_SETDESKWALLPAPER , 0 , path , SPIF_UPDATEINIFILE );
        }
    }
}
