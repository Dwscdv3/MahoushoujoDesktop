﻿using System;
using System . Collections . Generic;
using System . Linq;
using System . Runtime . InteropServices;
using System . Text;
using System . Threading . Tasks;

namespace MahoushoujoDesktop . Native
{
    public static class User32
    {
        public const uint SPI_SETDESKWALLPAPER = 0x14;
        public const uint SPIF_UPDATEINIFILE = 0x01;

        [DllImport ( "user32.dll" )]
        public static extern bool SystemParametersInfo ( uint uiAction , uint uiParam , string pvParam , uint fWinIni );

        [return: MarshalAs ( UnmanagedType . Bool )]
        [DllImport ( "user32.dll" , SetLastError = true , CharSet = CharSet . Auto )]
        public static extern bool PostMessage ( IntPtr hWnd , uint Msg , IntPtr wParam , IntPtr lParam );
    }
}
