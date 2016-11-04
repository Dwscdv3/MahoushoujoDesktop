using System;
using System . ComponentModel;
using System . Runtime . InteropServices;
using static MahoushoujoDesktop . Native . User32;

namespace MahoushoujoDesktop . Native
{
    public static class Managed
    {
        public static readonly IntPtr HWND_TOPMOST = new IntPtr ( -1 );
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr ( -2 );
        public static readonly IntPtr HWND_TOP = new IntPtr ( 0 );
        public static readonly IntPtr HWND_BOTTOM = new IntPtr ( 1 );

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
