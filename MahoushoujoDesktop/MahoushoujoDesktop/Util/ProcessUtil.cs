using System;

namespace MahoushoujoDesktop . Util
{
    public static class ProcessUtil
    {
        public static bool IsAnotherInstanceExist ()
        {
            var currentProcess = System . Diagnostics . Process . GetCurrentProcess ();
            var currentFileName = currentProcess . MainModule . FileName;
            var processes = System . Diagnostics . Process . GetProcessesByName ( currentProcess . ProcessName );
            if ( processes . Length > 1 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static System . Diagnostics . Process GetRunningInstance ()
        {
            System . Diagnostics . Process currentProcess = System . Diagnostics . Process . GetCurrentProcess ();
            string currentFileName = currentProcess . MainModule . FileName;
            System . Diagnostics . Process [] processes = System . Diagnostics . Process . GetProcessesByName ( currentProcess . ProcessName );
            foreach ( System . Diagnostics . Process process in processes )
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
