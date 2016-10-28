using System;
using System . Collections . Generic;
using System . Diagnostics;
using System . Linq;
using System . Net;
using System . Text;
using System . Threading;
using System . Threading . Tasks;

namespace MahoushoujoDesktop
{
    class Network
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
            WebClient c = getWebClient ();
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
    }
}
