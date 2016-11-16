using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using static MahoushoujoDesktop . Mahoushoujo;
using MahoushoujoDesktop . Util;
using System . Net;

namespace MahoushoujoDesktop
{
    public class ImageInfo
    {
        JsonImageInfo _info = null;
        public JsonImageInfo Info
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
        public string WeiboImageUrl
        {
            get
            {
                return GetWeiboImageUrl ( Info . 微博图片 );
            }
        }

        public ImageInfo ( JsonImageInfo info )
        {
            this . Info = info;
        }

        public static async Task<ImageInfo> GetImageInfoById ( int id )
        {
            var json = await CustomWebRequest . Get ( $"http://api.syouzyo.org/v2/img/{id}" );
            return new ImageInfo ( Json . ToObject<JsonImageInfo> ( json ) );
        }
    }
}
