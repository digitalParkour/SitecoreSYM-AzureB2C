using System;

namespace SYMB2C.Foundation.Caching
{
    public interface IStateManager
    {
        void Set<T>(string key, T value);

        void Set<T>(string key, T value, DateTime absoluteExpiration);

        void Set<T>(string key, T value, bool isCacheable);

        void Remove(string key);

        T Get<T>(string key);

        bool Exists(string key);

        void Clear();

        // void RemoveLike(string keyLike);
    }
}
