using System;
using System . Collections . Generic;
using System . Web . Script . Serialization;

namespace MahoushoujoDesktop . Util
{
    public static class Json
    {
        public static T ToObject<T> ( string json )
        {
            var serializer = new JavaScriptSerializer ();
            return serializer . Deserialize<T> ( json );
        }
        public static Dictionary<string , T> ToDictionary<T> ( string json )
        {
            var serializer = new JavaScriptSerializer ();
            return serializer . Deserialize<Dictionary<string , T>> ( json );
        }
    }
    public static class DictionaryExtension
    {
        public static Dictionary<int , T> ConvertKeysToInt32<T> ( this Dictionary<string , T> dict )
        {
            var newDict = new Dictionary<int , T> ();
            foreach ( var key in dict . Keys )
            {
                newDict . Add ( int . Parse ( key ) , dict [ key ] );
            }
            return newDict;
        }
    }
}
