using System;
using System . Diagnostics;

namespace MahoushoujoDesktop
{
    public static class SystemUtil
    {
        public static bool IsAnotherInstanceExist ()
        {
            var currentProcess = Process . GetCurrentProcess ();
            var currentFileName = currentProcess . MainModule . FileName;
            var processes = Process . GetProcessesByName ( currentProcess . ProcessName );
            if ( processes . Length > 1 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Process GetRunningInstance ()
        {
            Process currentProcess = Process . GetCurrentProcess ();
            string currentFileName = currentProcess . MainModule . FileName;
            Process [] processes = Process . GetProcessesByName ( currentProcess . ProcessName );
            foreach ( Process process in processes )
            {
                if ( process . MainModule . FileName == currentFileName )
                {
                    if ( process . Id != currentProcess . Id )
                        return process;
                }
            }
            return null;
        }
    }
}
