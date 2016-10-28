using System;
using System . Collections . Generic;

namespace MahoushoujoDesktop
{
    public class JsonImageInfo
    {
        public int id { get; set; }
        public string 标题 { get; set; }
        public string 微博图片 { get; set; }
        public string 颜色 { get; set; }
        public List<string> 颜色们 { get; set; }
        public int h { get; set; }
        public int s { get; set; }
        public int l { get; set; }
        public int 宽 { get; set; }
        public int 高 { get; set; }
        public double 比例 { get; set; }
        public string 文件名 { get; set; }
        public string 备份 { get; set; }
        public string 绘师 { get; set; }
        public int 状态 { get; set; }
        public List<JsonImageInfoTag> 标签们 { get; set; }
        public int 分级 { get; set; }
        public string 来源 { get; set; }
        public string 来源二 { get; set; }
        public int 抓取时间 { get; set; }
        public int 喜欢 { get; set; }
        public int created { get; set; }
        public int modified { get; set; }
        public JsonImageInfoUser user { get; set; }
    }
    public class JsonImageInfoUser
    {
        public int uid { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
    }
    public class JsonImageInfoTag
    {
        public int id { get; set; }
        public string 标签名 { get; set; }
        public string 翻译名 { get; set; }
        public int 状态 { get; set; }
        public int 数量 { get; set; }
    }
}
