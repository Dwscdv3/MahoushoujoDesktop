using System;
using System . Collections . Generic;
using System . Diagnostics;
using System . IO;
using System . Linq;
using System . Net;
using System . Text;
using System . Text . RegularExpressions;
using System . Threading . Tasks;
using System . Web . Script . Serialization;
using System . Windows . Media . Imaging;
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

        public User ( JsonUserInfo info )
        {
            this . Info = info;
        }

        public static async Task<User> LogIn ( string id , string password )
        {
            var token = await GetToken ( id , password );
            if ( !string . IsNullOrEmpty ( token ) )
            {
                var user = await GetUserInfoByToken ( token );
                if ( user . Info . uid > 0 )
                {
                    Default . UserAccessToken = token;
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

                var req = WebRequest . Create ( "http://api.mouto.org/x/?a=u" );

                req . Method = "POST";
                req . ContentType = "application/x-www-form-urlencoded";
                req . ContentLength = form . Length;

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

            var req = WebRequest . CreateHttp ( "http://api.syouzyo.org/?u" );

            req . Method = "POST";
            req . ContentType = "application/x-www-form-urlencoded";
            req . ContentLength = form . Length;
            req . Referer = "http://syouzyo.org/?from=Dwscdv3.WindowsClient";

            var s = await req . GetRequestStreamAsync ();
            await s . WriteAsync ( form , 0 , form . Length );
            s . Close ();

            var res = await req . GetResponseAsync ();
            var json = await new StreamReader ( res . GetResponseStream () , Encoding . UTF8 ) . ReadToEndAsync ();
            var parser = new JavaScriptSerializer ();
            var info = parser . Deserialize<JsonUserInfo> ( json );
            res . Close ();

            return new User ( info );
        }
        public static async Task<User> GetUserInfoById ( int id )
        {
            var req = WebRequest . CreateHttp ( $"http://api.syouzyo.org/?user/{id}" );
            req . Referer = "http://syouzyo.org/?from=Dwscdv3.WindowsClient";

            var res = await req . GetResponseAsync ();
            string json = await new StreamReader ( res . GetResponseStream () , Encoding . UTF8 ) . ReadToEndAsync ();
            var parser = new JavaScriptSerializer ();
            var info = parser . Deserialize<JsonUserInfo> ( json );
            res . Close ();

            return new User ( info );
        }
    }
}
