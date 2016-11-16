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
        private Dictionary<string , string> _customHeaders = new Dictionary<string , string> ();
        public Dictionary<string , string> CustomHeaders
        {
            get
            {
                return _customHeaders;
            }
            set
            {
                _customHeaders = value;
            }
        }

        public Network ()
        {

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

        private void setCustomHeaders ( HttpWebRequest req )
        {
            foreach ( var pair in CustomHeaders )
            {
                switch ( pair . Key )
                {
                case "Accept":
                    req . Accept = pair . Value;
                    break;
                case "Connection":
                    req . Connection = pair . Value;
                    break;
                case "Content-Length":
                    req . ContentLength = long . Parse ( pair . Value );
                    break;
                case "Content-Type":
                    req . ContentType = pair . Value;
                    break;
                case "Expect":
                    req . Expect = pair . Value;
                    break;
                case "Date":
                    req . Date = DateTime . Parse ( pair . Value );
                    break;
                case "Host":
                    req . Host = pair . Value;
                    break;
                case "If-Modified-Since":
                    req . IfModifiedSince = DateTime . Parse ( pair . Value );
                    break;
                case "Range":
                    throw new NotImplementedException ();
                case "Referer":
                    req . Referer = pair . Value;
                    break;
                case "Transfer-Encoding":
                    req . TransferEncoding = pair . Value;
                    break;
                case "User-Agent":
                    req . UserAgent = pair . Value;
                    break;
                default:
                    req . Headers . Add ( pair . Key , pair . Value );
                    break;
                }
            }
        }

        public async Task<byte []> WebClientGetBytes ( string url )
        {
            return await WebClientGetBytes ( url , null );
        }
        public async Task<byte []> WebClientGetBytes ( string url , DownloadProgressChangedEventHandler progressEvent )
        {
            WebClient c = getWebClient ();
            if ( progressEvent != null )
            {
                c . DownloadProgressChanged += progressEvent;
            }
            return await c . DownloadDataTaskAsync ( new Uri ( url ) );
        }
        public async Task<string> WebClientGetString ( string url )
        {
            return await WebClientGetString ( url , Encoding . UTF8 );
        }
        public async Task<string> WebClientGetString ( string url , Encoding encoding )
        {
            try
            {
                return encoding . GetString ( await WebClientGetBytes ( url ) );
            }
            catch ( Exception ex )
            {
                Debug . WriteLine ( ex + "\r\n\t" + ex . Message );
                return "";
            }
        }

        public async Task<string> Get ( string url )
        {
            return await Get ( url , Encoding . UTF8 );
        }
        public async Task<string> Get ( string url , Encoding encoding )
        {
            var req = WebRequest . CreateHttp ( url );
            setCustomHeaders ( req );

            var res = await req . GetResponseAsync ();
            string resContent = await new StreamReader ( res . GetResponseStream () , encoding ) . ReadToEndAsync ();
            res . Close ();

            return resContent;
        }
        public async Task<string> Post ( string url , byte [] form )
        {
            return await Post ( url , form , Encoding . UTF8 );
        }
        public async Task<string> Post ( string url , byte [] form , Encoding encoding )
        {
            var req = WebRequest . CreateHttp ( url );

            req . Method = "POST";
            req . ContentType = "application/x-www-form-urlencoded";
            setCustomHeaders ( req );

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
