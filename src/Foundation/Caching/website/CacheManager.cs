using System;
using SYMB2C.Foundation.DependencyInjection;
using Sitecore.Caching.Generics;

namespace SYMB2C.Foundation.Caching
{
    [Service(typeof(IStateManager), Lifetime = Lifetime.Singleton)]
    public class CacheManager : IStateManager
    {
        private static readonly int CacheDurationMinutes = Sitecore.Configuration.Settings.GetIntSetting("Foundation.Cache.DefaultDurationInMinutes", 5);

        private readonly ICache<string> cache = new Sitecore.Caching.Cache("symb2c-cache", 2024);

        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            if (this.Exists(key))
            {
                this.Remove(key);
            }

            this.cache.Add(key, value, absoluteExpiration);
        }

        public void Set<T>(string key, T value)
        {
            this.Set(key, value, DateTime.Now.AddMinutes(CacheDurationMinutes));
        }

        public void Set<T>(string key, T value, bool isCacheable)
        {
            if (isCacheable)
            {
                this.Set(key, value);
            }
        }

        public T Get<T>(string key)
        {
            return (T)this.cache.GetValue(key);
        }

        public void Remove(string key)
        {
            this.cache.Remove(key);
        }

        public bool Exists(string key)
        {
            return this.cache.ContainsKey(key);
        }

        public void Clear()
        {
            this.cache.Clear();
        }

        //public void RemoveLike(string keyLike)
        //{
        //    var keys = this.Cache.GetCacheKeys();

        //    foreach (var key in keys)
        //    {
        //        if (key != null && key.ToLower().Contains(keyLike.ToLower()))
        //        {
        //            this.Cache.Remove(key);
        //        }
        //    }
        //}
    }
}