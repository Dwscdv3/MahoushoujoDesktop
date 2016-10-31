using System;
using System . Diagnostics;
using System . IO;
using System . Windows;
using MahoushoujoDesktop . Properties;
using static MahoushoujoDesktop . Const;
using static MahoushoujoDesktop . Native . Managed;
using static MahoushoujoDesktop . SystemUtil;
using WM = MahoushoujoDesktop . Native . WindowMessage;

namespace MahoushoujoDesktop
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup ( object sender , StartupEventArgs e )
        {
            Environment . CurrentDirectory = AppDomain . CurrentDomain . SetupInformation . ApplicationBase;

            var process = GetRunningInstance ();
            if ( process != null )
            {
                try
                {
                    if ( File . Exists ( FileNameWindowHandle ) )
                    {
                        string handleString = File . ReadAllText ( FileNameWindowHandle );
                        IntPtr handle = new IntPtr ( long . Parse ( handleString ) );
                        // 备注：ShowInTaskbar 为 false 时无法从 Process 类获取主窗口句柄 - http://stackoverflow.com/a/5820666
                        PostMessageSafe ( handle , WM . Custom_ShowWindow , IntPtr . Zero , IntPtr . Zero );
                    }
                }
                catch ( Exception ex )
                {
                    Debug . WriteLine ( ex + "\r\n\t" + ex . Message );
                }
                finally
                {
                    Environment . Exit ( 1 );
                }
            }
        }

        private void Application_Exit ( object sender , ExitEventArgs e )
        {
            Settings . Default . Save ();

            if ( File . Exists ( FileNameWindowHandle ) )
            {
                File . Delete ( FileNameWindowHandle );
            }
        }
    }
}
