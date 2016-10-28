using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Web . Script . Serialization;
using MahoushoujoDesktop;
using static MahoushoujoDesktop . Const;

namespace MahoushoujoDesktop
{
    public static class Mahoushoujo
    {
        public static readonly Dictionary<string , string> MahoushoujoCustomHeaders = new Dictionary<string , string> ()
        {
            { "Referer", "http://syouzyo.org/?from=WindowsClient" }
        };

        static Network net;

        static Mahoushoujo ()
        {
            net = new Network ()
            {
                CustomHeaders = MahoushoujoCustomHeaders
            };
        }

        public static async Task<JsonImageInfo> GetRandom ()
        {
            string json = await net . GetString ( UrlApi + "rand&预设=宽屏" );
            JavaScriptSerializer parser = new JavaScriptSerializer ();
            var obj = parser . DeserializeObject ( json );
            return parser . ConvertToType<JsonImageInfo> ( obj );
        }
    }
}
