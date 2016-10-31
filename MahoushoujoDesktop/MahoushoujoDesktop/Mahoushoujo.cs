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
using static MahoushoujoDesktop . Const;
using static MahoushoujoDesktop . Native . Managed;
using static MahoushoujoDesktop . Properties . Settings;

namespace MahoushoujoDesktop
{
    public static class Mahoushoujo
    {
        public static readonly Dictionary<string , string> MahoushoujoCustomHeaders = new Dictionary<string , string> ()
        {
            { "Referer", "http://syouzyo.org/?from=WindowsClient" }
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
                timer . IsEnabled = value;
                Default . MainSwitch = value;
                Default . Save ();
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

        static Network net;
        static DispatcherTimer timer = new DispatcherTimer ();
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
            net = new Network ()
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
            if ( Default . MainSwitch )
            {
                timer . Start ();
            }
        }

        static void timer_Tick ( object sender , EventArgs e )
        {
            Next ();
        }
        public static void ResetTimerProgress ()
        {
            if ( IsTimerEnabled )
            {
                timer . Stop ();
                timer . Start ();
            }
        }
        public static void SetInfo ( JsonImageInfo info )
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

            SetWallpaper ( info );

            Default . LastImage = info;
            Default . Save ();
        }

        private static void IdLink_OnClick ( object sender , RoutedEventArgs e )
        {
            var link = (Hyperlink) sender;
            var run = link . Inlines . FirstOrDefault () as Run;
            string text = run == null ? string . Empty : run . Text;
            Process . Start ( "http://syouzyo.org/#/p/" + text );
        }
        private static void SourceLink_OnClick ( object sender , EventArgs e )
        {
            var link = (Hyperlink) sender;
            var run = link . Inlines . FirstOrDefault () as Run;
            string text = run == null ? string . Empty : run . Text;
            Process . Start ( text );
        }

        private static async void SetWallpaper ( JsonImageInfo info )
        {
            var path = await DownloadWeiboImage ( info . 微博图片 );

            await Task . Delay ( 50 );
            SetWallpaperLegacy ( path );
        }
        
        public static void ResetTimeFilter ()
        {
            time = 0;
        }

        public static async void Next ()
        {
            JsonImageInfo info = null;
            if ( pointInHistory >= history . Count - 1 )
            {
                // TODO: if Mode=Random then Random()
                string json;
                if ( time <= 0 || time >= 2000000000 )
                {
                    json = await net . GetString ( UrlApi + "img&count=1&h=-1&l=-1&比例=pc&unix=2000000000" );
                }
                else
                {
                    json = await net . GetString ( UrlApi + "img&count=1&h=-1&l=-1&比例=pc&unix=" + time . ToString () );
                }
                if ( !string . IsNullOrWhiteSpace ( json ) )
                {
                    JavaScriptSerializer parser = new JavaScriptSerializer ();
                    var obj = parser . DeserializeObject ( json );
                    info = parser . ConvertToType<List<JsonImageInfo>> ( obj ) [ 0 ];
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

        public static async Task<JsonImageInfo> Random ()
        {
            string json = await net . GetString ( UrlApi + "rand&预设=宽屏" );
            JavaScriptSerializer parser = new JavaScriptSerializer ();
            var obj = parser . DeserializeObject ( json );
            var info = parser . ConvertToType<JsonImageInfo> ( obj );
            history . Add ( info );
            return info;
        }

        public static async Task<string> DownloadWeiboImage ( string hash )
        {
            IsDownloading = true;

            var data = await net . GetBytes ( getWeiboImageUrl ( hash ) , updateProgressBar );
            var path = FileNameCurrentImage;
            FileInfo file = new FileInfo ( path );
            FileStream stream = null;
            if ( file . Exists )
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

            IsDownloading = false;

            return Environment . CurrentDirectory + "\\" + path;
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
}
