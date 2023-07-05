using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.Application.Cache.Impl
{
    public class MemoryCacheService : ICacheService
    {
        protected IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool Add(string key, object value, int ExpirtionTime = 120)
        {
            if (!string.IsNullOrEmpty(key))
            {
                MemoryCacheEntryOptions cacheEntityOps = new MemoryCacheEntryOptions()
                {
                    //滑动过期时间 120秒没有访问则清除
                    SlidingExpiration = TimeSpan.FromSeconds(ExpirtionTime),
                    //设置份数
                    Size = 1,
                    //优先级
                    Priority = CacheItemPriority.Low,
                };
                //过期回掉
                cacheEntityOps.RegisterPostEvictionCallback((keyInfo, valueInfo, reason, state) =>
                {
                    Console.WriteLine($"回调函数输出【键:{keyInfo},值:{valueInfo},被清除的原因:{reason}】");
                });
                _cache.Set(key, value, cacheEntityOps);
            }
            return true;
        }

        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            if (Exists(key))
            {
                _cache.Remove(key);
                return true;
            }
            return false;
        }

        public string GetValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            if (Exists(key))
            {
                return _cache.Get(key).ToString();
            }
            return null;
        }

        public bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            object cache;
            return _cache.TryGetValue(key, out cache);
        }

    }
}
