using System;
using System . Collections . Generic;
using System . Linq;
using System . Net;
using System . Text;
using System . Threading . Tasks;
using MahoushoujoDesktop . DataModel;
using MahoushoujoDesktop . Util;
using static MahoushoujoDesktop . Mahoushoujo;

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

        public async Task<byte []> GetBytes ()
        {
            byte [] data = null;
            for ( int i = 0 ; i < 3 ; i++ )
            {
                try
                {
                    data = await CustomHttpClient . GetByteArrayAsync ( GetWeiboImageUrl ( Info . 微博图片 ) );
                    break;
                }
                catch ( Exception )
                {
                    continue;
                }
            }
            if ( data == null && !string . IsNullOrEmpty ( Info . 备份 ) )
            {
                for ( int i = 0 ; i < 2 ; i++ )
                {
                    try
                    {
                        data = await CustomHttpClient . GetByteArrayAsync ( Info . 备份 );
                        break;
                    }
                    catch ( Exception )
                    {
                        continue;
                    }
                }
            }
            return data;
        }

        public static async Task<ImageInfo> GetImageInfoById ( int id )
        {
            var json = await CustomWebRequest . Get ( $"http://api.syouzyo.org/v2/img/{id}" );
            return new ImageInfo ( Json . ToObject<JsonImageInfo> ( json ) );
        }
    }
}
