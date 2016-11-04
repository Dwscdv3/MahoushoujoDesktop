using System;
using System . Collections . Generic;
using System . IO;
using System . Linq;
using System . Net;
using System . Text;
using System . Text . RegularExpressions;
using System . Threading . Tasks;

namespace MahoushoujoDesktop
{
    public static class User
    {
        public static async Task<string> GetToken ( string id , string password )
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

            return Regex . Match ( cookieString , "(?<=sss=).+?(?=;)" ) . Value;
        }

        public static async Task<string> GetUserInfo ( string token )
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
            string json = await new StreamReader ( res . GetResponseStream () , Encoding . UTF8 ) . ReadToEndAsync ();
            res . Close ();

            return json;
        }
    }
}
