using System;
using System . Runtime . InteropServices;
using static MahoushoujoDesktop . Native . Managed;

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

        [DllImport ( "user32.dll" , SetLastError = true )]
        public static extern bool SetWindowPos ( IntPtr hWnd , IntPtr hWndInsertAfter , int X , int Y , int cx , int cy , SetWindowPosFlags uFlags );
        [DllImport ( "user32.dll" , SetLastError = true , CharSet = CharSet . Auto )]
        public static extern bool SendNotifyMessage ( IntPtr hWnd , uint Msg , UIntPtr wParam , IntPtr lParam );
        [DllImport ( "user32.dll" )]
        public static extern int SetWindowLong ( IntPtr hWnd , int nIndex , int dwNewLong );
        [DllImport ( "user32.dll" , SetLastError = true )]
        public static extern int GetWindowLong ( IntPtr hWnd , int nIndex );
    }
}
