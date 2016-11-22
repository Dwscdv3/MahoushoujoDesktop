using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . ComponentModel;

// TODO: 旧版数据格式，不支持数据绑定
namespace MahoushoujoDesktop . DataModel
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
        public List<JsonTagInfo> 标签们 { get; set; }
        public int 分级 { get; set; }
        public string 来源 { get; set; }
        public string 来源二 { get; set; }
        public int 抓取时间 { get; set; }
        public int 喜欢 { get; set; }
        public int created { get; set; }
        public int modified { get; set; }
        public JsonShortUserInfo user { get; set; }
    }
    public class JsonTagInfo
    {
        public int id { get; set; }
        public string 标签名 { get; set; }
        public string 翻译名 { get; set; }
        public string 别名 { get; set; }
        public int 数量 { get; set; }
        public int 状态 { get; set; }
        public int 喜欢 { get; set; }
        public int created { get; set; }
        public int modified { get; set; }
    }
    public class JsonShortTagInfo
    {
        public int id { get; set; }
        public string 标签名 { get; set; }
        public string 翻译名 { get; set; }
        public int 状态 { get; set; }
        public int 数量 { get; set; }
    }
    public class JsonUserInfo
    {
        public int uid { get; set; }
        public string wbid { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
        public string des { get; set; }
        public string url { get; set; }
        public int lv { get; set; }
        public int up { get; set; }
        public string sss { get; set; }
        public int 喜欢数 { get; set; }
        public int 上传数 { get; set; }
        public int 专辑数 { get; set; }
    }
    public class JsonShortUserInfo
    {
        public int uid { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
    }
    public class JsonAlbumInfo
    {
        public int id { get; set; }
        public int uid { get; set; }
        public string 标题 { get; set; }
        public string 简介 { get; set; }
        //public List<JsonImageInfo> 图片们 { get; set; }
        public string 封面图 { get; set; }
        public string 颜色 { get; set; }
        public int 状态 { get; set; }
        public int 图片数 { get; set; }
        public int 关注人数 { get; set; }
        public int 查看数 { get; set; }
        public int 点赞数 { get; set; }
        public int created { get; set; }
        public int modified { get; set; }
    }
}
