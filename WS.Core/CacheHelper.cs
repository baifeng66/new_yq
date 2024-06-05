using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace WS.Core
{
    /// <summary>
    /// 功能描述    ：CacheHelper  
    /// 创 建 者    ：pc
    /// 创建日期    ：2020/11/17 9:34:31 
    /// 最后修改者  ：pc
    /// 最后修改日期：2020/11/17 9:34:31 
    /// </summary>
    public static class CacheHelper
    {

        private static IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions() { SizeLimit = 5000, ExpirationScanFrequency = TimeSpan.FromSeconds(5) });
        /// <summary>
        /// 增加或覆盖缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void Add(string key, object val)
        {
            //var _entry = _cache.CreateEntry(key);
            //_entry.Value = val;
            //_entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);//最长1小时缓存过期
            //_entry.SlidingExpiration = TimeSpan.FromMinutes(2);//10分钟未访问过期
            if (key != null)
            {
                _cache.Set(key, val, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(2),//10分钟未访问过期
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),//最长1小时缓存过期
                    Size = 1
                });
            }
        }

        public static object Get(string key)
        {
            object obj;
            var b = _cache.TryGetValue(key, out obj);
            if (b)
            {
                return obj;
            }
            else
            {
                return null;
            }
        }
        public static void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
