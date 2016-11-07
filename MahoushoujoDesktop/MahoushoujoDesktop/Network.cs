using System;
using System . Collections . Generic;
using System . Diagnostics;
using System . IO;
using System . Linq;
using System . Net;
using System . Text;
using System . Threading;
using System . Threading . Tasks;

namespace MahoushoujoDesktop
{
    public class Network
    {
        public Dictionary<string , string> CustomHeaders = new Dictionary<string , string> ();

        WebClient webClient = new WebClient ();
        public event DownloadProgressChangedEventHandler DownloadProgressChanged;

        public Network ()
        {
            webClient . DownloadProgressChanged += ( sender , e ) =>
              {
                  DownloadProgressChanged?.Invoke ( sender , e );
              };
        }

        WebClient getWebClient ()
        {
            WebClient c = new WebClient ();
            foreach ( var pair in CustomHeaders )
            {
                c . Headers . Add ( pair . Key , pair . Value );
            }
            return c;
        }

        public async Task<byte []> GetBytes ( string url )
        {
            return await GetBytes ( url , null );
        }
        public async Task<byte []> GetBytes ( string url , DownloadProgressChangedEventHandler progressEvent )
        {
            WebClient c = getWebClient ();
            if ( progressEvent != null )
            {
                c . DownloadProgressChanged += progressEvent;
            }
            return await c . DownloadDataTaskAsync ( new Uri ( url ) );
        }
        public async Task<string> GetString ( string url )
        {
            return await GetString ( url , Encoding . UTF8 );
        }
        public async Task<string> GetString ( string url , Encoding encoding )
        {
            try
            {
                return encoding . GetString ( await GetBytes ( url ) );
            }
            catch ( Exception ex )
            {
                Debug . WriteLine ( ex + "\r\n\t" + ex . Message );
                return "";
            }
        }
        
        public async Task<string> Post ( string url , byte [] form )
        {
            return await Post ( url , form , Encoding . UTF8 );
        }
        public async Task<string> Post ( string url , byte [] form , Encoding encoding )
        {
            var req = WebRequest . Create ( url );

            req . Method = "POST";
            req . ContentType = "application/x-www-form-urlencoded";
            req . ContentLength = form . Length;
            foreach ( var pair in CustomHeaders )
            {
                req . Headers . Add ( pair . Key , pair . Value );
            }

            var reqStream = await req . GetRequestStreamAsync ();
            await reqStream . WriteAsync ( form , 0 , form . Length );
            reqStream . Close ();

            var res = await req . GetResponseAsync ();
            string resContent = await new StreamReader ( res . GetResponseStream () , encoding ) . ReadToEndAsync ();
            res . Close ();

            return resContent;
        }
    }
}
