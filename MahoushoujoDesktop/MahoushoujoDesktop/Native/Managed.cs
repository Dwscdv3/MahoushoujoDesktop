using System;
using System . ComponentModel;
using System . Runtime . InteropServices;
using static MahoushoujoDesktop . Native . User32;

namespace MahoushoujoDesktop . Native
{
    public static class Managed
    {
        public static bool SetWallpaperLegacy ( string path )
        {
            return SystemParametersInfo ( SPI_SETDESKWALLPAPER , 0 , path , SPIF_UPDATEINIFILE );
        }

        public static void PostMessageSafe ( IntPtr hWnd , uint msg , IntPtr wParam , IntPtr lParam )
        {
            bool returnValue = PostMessage ( hWnd , msg , wParam , lParam );
            if ( !returnValue )
            {
                // An error occured
                throw new Win32Exception ( Marshal . GetLastWin32Error () );
            }
        }

    }
}
