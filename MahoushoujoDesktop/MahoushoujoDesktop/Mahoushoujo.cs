using System;
using System . Collections . Generic;
using System . Diagnostics;
using System . IO;
using System . Linq;
using System . Net;
using System . Text;
using System . Threading . Tasks;
using System . Web . Script . Serialization;
using System . Windows;
using System . Windows . Documents;
using System . Windows . Media;
using System . Windows . Navigation;
using System . Windows . Threading;
using MahoushoujoDesktop . Util;
using static MahoushoujoDesktop . Const;
using static MahoushoujoDesktop . Native . Managed;
using static MahoushoujoDesktop . Properties . Settings;

namespace MahoushoujoDesktop
{
    public static class Mahoushoujo
    {
        public static readonly Dictionary<string , string> MahoushoujoCustomHeaders = new Dictionary<string , string> ()
        {
            { "Referer", "http://syouzyo.org/?from=Dwscdv3.WindowsClient" }
        };

        static bool _isDownloading = false;
        public static bool IsDownloading
        {
            get
            {
                return _isDownloading;
            }
            set
            {
                _isDownloading = value;
                if ( CanGoPrevious )
                {
                    mainWindow . buttonPrev . IsEnabled = !value;
                }
                else
                {
                    mainWindow . buttonPrev . IsEnabled = false;
                }
                mainWindow . buttonNext . IsEnabled = !value;
            }
        }
        public static bool IsTimerEnabled
        {
            get
            {
                return timer . IsEnabled;
            }
            set
            {
                if ( value )
                {
                    timerStartTime = DateTime . Now;
                }
                timerProgressBarNext . IsEnabled = value;
                mainWindow . progressBarNext . Value = 0;
                timer . IsEnabled = value;
                Default . MainSwitch = value;
                mainWindow . buttonMainSwitch . IsChecked = value;
            }
        }
        public static bool CanGoPrevious
        {
            get
            {
                return pointInHistory > 0;
            }
        }
        static Source _source = Source . Index;
        public static Source Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
                Reset ();
            }
        }
        static bool _isRandom = false;
        public static bool IsRandom
        {
            get
            {
                return _isRandom;
            }
            set
            {
                if ( value != _isRandom )
                {
                    _isRandom = value;
                    Reset ();
                }
            }
        }
        public static TimeSpan TimerInterval
        {
            get
            {
                return timer . Interval;
            }
            set
            {
                timer . Interval = value;
                ResetTimerProgress ();
                //Default . TimerInterval = value;
            }
        }

        public static Network NetMahoushoujo;
        static User _logInUser;
        public static User LogInUser
        {
            get
            {
                return _logInUser;
            }
            set
            {
                _logInUser = value;
                Default . UserAccessToken = value?.Info?.sss;
                mainWindow . SetUserPanel ();
            }
        }
        public static bool IsLoggedIn
        {
            get
            {
                return LogInUser != null;
            }
        }

        static DispatcherTimer timer = new DispatcherTimer ();
        static DateTime timerStartTime;
        static DispatcherTimer timerProgressBarNext = new DispatcherTimer ();
        static MainWindow mainWindow;
        static int time = 0;
        static List<JsonImageInfo> history = new List<JsonImageInfo> ();
        static int pointInHistory = -1;

        static Mahoushoujo ()
        {

        }

        public static void Init ()
        {
            mainWindow = (MainWindow) App . Current . MainWindow;
            NetMahoushoujo = new Network ()
            {
                CustomHeaders = MahoushoujoCustomHeaders
            };

            if ( Default . LastImage != null )
            {
                history . Add ( Default . LastImage );
                pointInHistory = 0;
                SetInfo ( Default . LastImage );
            }

            timer . Interval = Default . TimerInterval;
            timer . Tick += timer_Tick;
            timerProgressBarNext . Interval = TimeSpan . FromMilliseconds ( 100 );
            timerProgressBarNext . Tick += TimerProgressBarNext_Tick;
            if ( Default . MainSwitch )
            {
                timerStartTime = DateTime . Now;
                timer . Start ();
                timerProgressBarNext . Start ();
            }
        }

        public static void Reset ()
        {
            ResetTimerProgress ();
            ResetTimeFilter ();
            history = new List<JsonImageInfo> ();
            pointInHistory = -1;
            timer_Tick ( null , null );
        }

        static void timer_Tick ( object sender , EventArgs e )
        {
            timerStartTime = DateTime . Now;

            if ( IsRandom )
            {
                Random ();
            }
            else
            {
                Next ();
            }
        }
        private static void TimerProgressBarNext_Tick ( object sender , EventArgs e )
        {
            mainWindow . progressBarNext . Value = ( DateTime . Now - timerStartTime ) . TotalMilliseconds / timer . Interval . TotalMilliseconds;
        }

        public static void ResetTimeFilter ()
        {
            time = 0;
        }
        public static void ResetTimerProgress ()
        {
            if ( IsTimerEnabled )
            {
                timer . Stop ();
                timerStartTime = DateTime . Now;
                timer . Start ();
            }
        }

        public static async void SetInfo ( JsonImageInfo info )
        {
            #region 详细信息::ID
            mainWindow . textId . Inlines . Clear ();
            var linkId = new Hyperlink ();
            linkId . Click += IdLink_OnClick;
            linkId . Inlines . Add ( info . id . ToString () );
            mainWindow . textId . Inlines . Add ( linkId );
            #endregion
            mainWindow . textTitle . Text = info . 标题;
            mainWindow . textAuthor . Text = info . 绘师;
            #region 详细信息::来源
            mainWindow . textSource . Text = "";
            mainWindow . textSource . Inlines . Clear ();
            if ( info . 来源 . StartsWith ( "http://" ) || info . 来源 . StartsWith ( "https://" ) )
            {
                var linkSource = new Hyperlink ();
                linkSource . Click += SourceLink_OnClick;
                linkSource . Inlines . Add ( info . 来源 );
                mainWindow . textSource . Inlines . Add ( linkSource );
            }
            else
            {
                mainWindow . textSource . Text = info . 来源;
            }
            #endregion
            #region 窗口::背景（暂废弃）
            //if ( string . IsNullOrWhiteSpace ( info . 颜色 ) )
            //{
            //    ( (LinearGradientBrush) mainWindow . Background ) . GradientStops [ 1 ] . Color =
            //        Color . FromArgb ( 191 , 0 , 0 , 0 );
            //}
            //else
            //{
            //    var color = Color . FromArgb ( 191 ,
            //        Convert . ToByte ( info . 颜色 . Substring ( 0 , 2 ) , 16 ) ,
            //        Convert . ToByte ( info . 颜色 . Substring ( 0 , 2 ) , 16 ) ,
            //        Convert . ToByte ( info . 颜色 . Substring ( 0 , 2 ) , 16 )
            //    );
            //    ( (LinearGradientBrush) mainWindow . Background ) . GradientStops [ 1 ] . Color = color;
            //}
            #endregion

            var succeeded = await DownloadAndSetWallpaper ( info );

            Default . LastImage = info;
        }
        private static async Task<bool> DownloadAndSetWallpaper ( JsonImageInfo info )
        {
            DownloadResult result;
            int retry = 0;
            do
            {
                result = await SaveWeiboImage ( info . 微博图片 );
                retry++;
            } while ( result == DownloadResult . Failed && retry < 3 );

            if ( result != DownloadResult . Succeeded )
            {
                if ( !string . IsNullOrWhiteSpace ( info . 备份 ) )
                {
                    retry = 0;
                    do
                    {
                        result = await SaveBackupImage ( info . 备份 );
                        retry++;
                    } while ( result == DownloadResult . Failed && retry < 2 );
                }
            }

            if ( result == DownloadResult . Succeeded )
            {
                await Task . Delay ( 50 );
                SetWallpaperLegacy ( Environment . CurrentDirectory + "\\" + FileNameCurrentImage );
                return true;
            }

            return false;
        }

        private static void IdLink_OnClick ( object sender , RoutedEventArgs e )
        {
            var link = (Hyperlink) sender;
            var run = link . Inlines . FirstOrDefault () as Run;
            string text = run == null ? string . Empty : run . Text;
            System . Diagnostics . Process . Start ( "http://syouzyo.org/#/p/" + text );
        }
        private static void SourceLink_OnClick ( object sender , EventArgs e )
        {
            var link = (Hyperlink) sender;
            var run = link . Inlines . FirstOrDefault () as Run;
            string text = run == null ? string . Empty : run . Text;
            System . Diagnostics . Process . Start ( text );
        }

        public static async void Next ()
        {
            JsonImageInfo info = null;
            if ( pointInHistory >= history . Count - 1 )
            {
                if ( IsRandom )
                {
                    Random ();
                    return;
                }

                string json;
                if ( time <= 0 || time >= 2000000000 )
                {
                    json = await NetMahoushoujo . GetString ( UrlApi + "img&count=1&h=-1&l=-1&比例=pc&unix=2000000000" );
                }
                else
                {
                    json = await NetMahoushoujo . GetString ( UrlApi + "img&count=1&h=-1&l=-1&比例=pc&unix=" + time . ToString () );
                }

                if ( !string . IsNullOrWhiteSpace ( json ) )
                {
                    info = Json . ToObject<JsonImageInfo []> ( json ) [ 0 ];
                    time = info . created;
                    history . Add ( info );
                    pointInHistory = history . Count - 1;
                }
            }
            else
            {
                info = history [ ++pointInHistory ];
            }
            if ( info != null )
            {
                SetInfo ( info );
            }
            else
            {
                Debug . WriteLine ( "Mahoushoujo.Next() info is null" );
            }
        }
        public static void Previous ()
        {
            if ( CanGoPrevious )
            {
                var info = history [ --pointInHistory ];
                SetInfo ( info );
            }
        }
        public static async void Random ()
        {
            string json = await NetMahoushoujo . GetString ( UrlApi + "rand&预设=宽屏" );
            var info = Json . ToObject<JsonImageInfo []> ( json ) [ 0 ];
            time = info . created;
            history . Add ( info );
            pointInHistory = history . Count - 1;
            if ( info != null )
            {
                SetInfo ( info );
            }
            else
            {
                Debug . WriteLine ( "Mahoushoujo.Random() info is null" );
            }
        }

        private static async Task<DownloadResult> SaveWeiboImage ( string hash )
        {
            byte [] data = null;

            data = await DownloadData ( getWeiboImageUrl ( hash ) );
            if ( data == null )
            {
                return DownloadResult . Failed;
            }
            if ( data . Length < 10000 )
            {
                return DownloadResult . Banned;
            }
            SaveImage ( data );

            return DownloadResult . Succeeded;
        }
        private static async Task<DownloadResult> SaveBackupImage ( string url )
        {
            byte [] data = null;

            data = await DownloadData ( url );
            if ( data == null )
            {
                return DownloadResult . Failed;
            }
            SaveImage ( data );

            return DownloadResult . Succeeded;
        }

        private static void SaveImage ( byte [] data )
        {
            var path = FileNameCurrentImage;
            var file = new FileInfo ( path );
            FileStream stream = null;
            if ( File . Exists ( path ) )
            {
                stream = file . OpenWrite ();
            }
            else
            {
                stream = file . Create ();
            }
            stream . Write ( data , 0 , data . Length );
            stream . Flush ( true );
            stream . Close ();
            file . Attributes = file . Attributes | FileAttributes . Hidden;
        }

        private static async Task<byte []> DownloadData ( string url )
        {
            byte [] data;
            IsDownloading = true;
            try
            {
                data = await NetMahoushoujo . GetBytes ( url , updateProgressBar );
            }
            catch ( WebException ex )
            {
                Debug . WriteLine ( ex + "\r\n\t" + ex . Message );
                IsDownloading = false;
                return null;
            }
            finally
            {
                IsDownloading = false;
            }
            return data;
        }
        static string getWeiboImageUrl ( string hash )
        {
            return $"http://ww1.sinaimg.cn/large/{hash}";
        }

        static void updateProgressBar ( object sender , DownloadProgressChangedEventArgs e )
        {
            if ( e . BytesReceived >= e . TotalBytesToReceive )
            {
                mainWindow . progressBar . Value = 0;
            }
            else
            {
                mainWindow . progressBar . Value = (double) e . BytesReceived / e . TotalBytesToReceive;
            }
        }
    }
    public enum Source
    {
        Index,
        Tag,
        Album,
        UserLike,
        Random = int . MaxValue, // TODO: Under consideration
        Icarus = -1
    }
    public enum DownloadResult
    {
        Succeeded,
        Failed,
        Banned
    }
}
