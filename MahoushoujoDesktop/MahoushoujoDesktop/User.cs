using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . Diagnostics;
using System . IO;
using System . Linq;
using System . Net;
using System . Text;
using System . Text . RegularExpressions;
using System . Threading . Tasks;
using System . Web . Script . Serialization;
using System . Windows . Media . Imaging;
using MahoushoujoDesktop . DataModel;
using MahoushoujoDesktop . Util;
using static MahoushoujoDesktop . Const;
using static MahoushoujoDesktop . Properties . Settings;

namespace MahoushoujoDesktop
{
    public class User
    {
        JsonUserInfo _info = null;
        public JsonUserInfo Info
        {
            get
            {
                return _info;
            }
            private set
            {
                _info = value;
            }
        }
        ObservableCollection<AlbumInfo> _albums = null;
        public ObservableCollection<AlbumInfo> Albums
        {
            get
            {
                return _albums;
            }
            private set
            {
                _albums = value;
            }
        }

        public User ( JsonUserInfo info )
        {
            this . Info = info;
            initAlbums ();
        }

        async void initAlbums ()
        {
            Albums = new ObservableCollection<AlbumInfo> ();
            var json = await Mahoushoujo . CustomHttpClient . GetStringAsync (
                UrlApiV2 + "album?uid=" + Info . uid . ToString () );
            var jsonObjects = Json . ToObject<List<JsonAlbumInfo>> ( json );
            foreach ( var jsonObject in jsonObjects )
            {
                AlbumInfo album = new AlbumInfo ();
                album . Name = jsonObject . 标题;
                album . Count = jsonObject . 图片数;
                album . Id = jsonObject . id;
                Albums . Add ( album );
            }
        }

        public static async Task<User> LogIn ( string id , string password )
        {
            var token = await GetToken ( id , password );
            if ( !string . IsNullOrEmpty ( token ) )
            {
                var user = await GetUserInfoByToken ( token );
                if ( user . Info . uid > 0 )
                {
                    return user;
                }
            }

            return null;
        }

        private static async Task<string> GetToken ( string id , string password )
        {
            try
            {
                var form = Encoding . UTF8 . GetBytes ( $"id={ Uri . EscapeDataString ( id ) }&password={ Uri . EscapeDataString ( password ) }" );

                var req = WebRequest . CreateHttp ( "http://api.mouto.org/x/?a=u" );

                req . Method = "POST";
                req . ContentType = "application/x-www-form-urlencoded";

                var s = await req . GetRequestStreamAsync ();
                await s . WriteAsync ( form , 0 , form . Length );
                s . Close ();

                var res = await req . GetResponseAsync ();
                var cookieString = res . Headers [ "Set-Cookie" ];
                res . Close ();

                if ( cookieString != null )
                {
                    return Regex . Match ( cookieString , "(?<=sss=).+?(?=;)" ) . Value;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        public static async Task<User> GetUserInfoByToken ( string token )
        {
            var form = Encoding . UTF8 . GetBytes ( $"sss={ token }" );
            var json = await Mahoushoujo . CustomWebRequest . Post ( UrlApiV1 + "u" , form );
            var info = Json . ToObject<JsonUserInfo> ( json );

            return new User ( info );
        }
        public static async Task<User> GetUserInfoById ( int id )
        {
            var json = await Mahoushoujo . CustomWebRequest . Get ( $"{UrlApiV1}user/{id}" );
            var info = Json . ToObject<JsonUserInfo> ( json );

            return new User ( info );
        }
    }
}
