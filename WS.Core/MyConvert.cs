using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WS.Core
{
    /// <summary>
    /// 功能描述    ：Mapper  
    /// 创 建 者    ：pc
    /// 创建日期    ：2020/11/20 14:12:59 
    /// 最后修改者  ：pc
    /// 最后修改日期：2020/11/20 14:12:59 
    /// </summary>
    public static class MyConvert
    {
        /// <summary>
        /// 移除字符串中的SQL注入字符
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string TrimSql(this string sql)
        {
            string[] in_str = { "'", "and", "exec", "insert", "select", "delete", "update", "count", "%", "chr", "mid", "master", "truncate", "char", "declare", ";", "or", "," };
            if (string.IsNullOrEmpty(sql))
            {
                return "";
            }
            for (int i = 0; i < in_str.Length; i++)
            {
                sql = Regex.Replace(sql, in_str[i], "", RegexOptions.IgnoreCase);
            }
            return sql;
        }

        /// <summary>
        /// 数据实体类型转换
        /// </summary>
        /// <typeparam name="O">输出类型</typeparam>
        /// <typeparam name="I">输入类型</typeparam>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static O Mapper<O, I>(I mode)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<O>(Newtonsoft.Json.JsonConvert.SerializeObject(mode));
        }

        /// <summary>
        /// 返回字符串的decimal形式，错误将返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string str)
        {
            decimal rt;
            bool b = decimal.TryParse(str, out rt);
            if (b)
            {
                return rt;
            }
            else
            {
                return 0M;
            }
        }
        /// <summary>
        /// 返回字符串的decimal形式，错误将返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(this string str)
        {
            float rt;
            bool b = float.TryParse(str, out rt);
            if (b)
            {
                return rt;
            }
            else
            {
                return 0f;
            }
        }

        /// <summary>
        /// 转换时间格式;
        /// 将yyyyMMddHHmmss格式转换为任意格式
        /// </summary>
        /// <param name="time14">yyyyMMddHHmmss格式</param>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static string ToDateStr(this string time14, string fmt)
        {
            if (string.IsNullOrEmpty(time14))
            {
                return "";
            }
            return DateTime.Parse(time14.Substring(0, 4) + "-" + time14.Substring(4, 2) + "-" + time14.Substring(6, 2) + " " + time14.Substring(8, 2) + ":" + time14.Substring(10, 2) + ":" + time14.Substring(12, 2)).ToString(fmt);
        }

        /// <summary>
        /// 转换时间格式;
        /// 将yyyyMMddHHmmss格式转换为任意格式
        /// </summary>
        /// <param name="time14">yyyyMMddHHmmss格式</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string time14)
        {
            if (string.IsNullOrEmpty(time14))
            {
                return DateTime.MinValue;
            }
            return DateTime.Parse(time14.Substring(0, 4) + "-" + time14.Substring(4, 2) + "-" + time14.Substring(6, 2) + " " + time14.Substring(8, 2) + ":" + time14.Substring(10, 2) + ":" + time14.Substring(12, 2));
        }
        public static long DateTimeToTimestamp(this DateTime dateTime)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = dateTime - unixEpoch;
            return (long)timeSpan.TotalSeconds;
        }

        ///// <summary>
        ///// base64字符串转图片
        ///// </summary>
        ///// <param name="base64String"></param>
        ///// <returns></returns>
        //public static Image ConvertBase64ToImage(this string base64String)
        //{
        //    base64String = base64String.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "")
        //               .Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "").Replace("data:image/bmp;base64,", ""); //将base64头部信息替换
        //    byte[] imageBytes = Convert.FromBase64String(base64String);
        //    using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
        //    {
        //        ms.Write(imageBytes, 0, imageBytes.Length);
        //        return Image.FromStream(ms, true);
        //    }
        //}

        ///// <summary>
        ///// 图片转base64
        ///// </summary>
        ///// <param name="file"></param>
        ///// <returns></returns>
        //public static string ConvertImageToBase64(this Image file)
        //{
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        file.Save(memoryStream, file.RawFormat);
        //        byte[] imageBytes = memoryStream.ToArray();
        //        return Convert.ToBase64String(imageBytes);
        //    }
        //}

        ///// <summary>
        ///// 获取图片的缩略图
        ///// </summary>
        ///// <param name="img">原始图像</param>
        ///// <param name="size">缩放后最大边的宽度</param>
        ///// <returns></returns>
        //public static Image GetThumbimg(this Image img, int size)
        //{
        //    float ratio = 1.0f;
        //    int sourcewidth = img.Width;
        //    int sourceheight = img.Height;
        //    if (sourceheight > sourcewidth && sourceheight > size)
        //    {
        //        ratio = size * 1.0f / sourceheight;
        //    }
        //    else if (sourcewidth > sourceheight && sourcewidth > size)
        //    {
        //        ratio = size * 1.0f / sourcewidth;
        //    }

        //    Image thumbimg = img.GetThumbnailImage(Convert.ToInt32(sourcewidth * ratio), Convert.ToInt32(sourceheight * ratio), () => { return false; }, IntPtr.Zero);

        //    return thumbimg;
        //}
        /// <summary>
        /// 当期日期所在周的最后一天
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime DayOfEndweek(this DateTime now) {
            int weeknow = Convert.ToInt32(now.DayOfWeek);
            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。
            // weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = 7 - weeknow;
            //本周最后一天
            DateTime lastday = Convert.ToDateTime(now.AddDays(daydiff).ToString("yyyy-MM-dd 23:59:59"));
            return lastday;
        }
        /// <summary>
        /// 当期日期所在周的第一天的日期
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime DayOfStartweek(this DateTime now)
        {
            int weeknow = Convert.ToInt32(now.DayOfWeek);
            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;
            //本周第一天
            DateTime firstDay = Convert.ToDateTime(now.AddDays(daydiff).ToString("yyyy-MM-dd 00:00:00"));
            return firstDay;
        }
        /// <summary>
        /// 获取指定日期，在为一年中为第几周
        /// </summary>
        /// <param name="dt">指定时间</param>
        /// <reutrn>返回第几周</reutrn>
        public static int WeekOfYear(this DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }

        /// <summary>
        /// 数据接口日志类型
        /// </summary>
        //public enum ApiLogType
        //{
           
        //}
    }
}
