using SYMB2C.Foundation.Consumer.Abstractions.Models.Caching;

namespace SYMB2C.Foundation.Consumer.Services
{
    public abstract class CacheableService
    {
        public virtual bool IsCacheable => true;

        protected virtual string CacheKeyPrefix => "service-";

        public virtual string GetKey(object obj = null)
        {
            string key;

            var cacheable = obj as ICacheableModel;

            if (cacheable != null)
            {
                key = $"{this.CacheKeyPrefix}{System.Reflection.MethodBase.GetCurrentMethod().Name}-{cacheable.GetCacheableKey()}";
                return key.ToLower();
            }

            key = $"{this.CacheKeyPrefix}{System.Reflection.MethodBase.GetCurrentMethod().Name}-{obj}";

            return key.ToLower();
        }
    }
}