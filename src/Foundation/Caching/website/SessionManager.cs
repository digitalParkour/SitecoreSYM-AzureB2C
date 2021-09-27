using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using SYMB2C.Foundation.DependencyInjection;
using Sitecore.Diagnostics;

namespace SYMB2C.Foundation.Caching
{
    /// <summary>
    /// Comments: All keys are prefixed with a string, so all session keys managed by this manager can be cleared in one call.
    /// </summary>
    [Service(typeof(IStateManager), Lifetime = Lifetime.Singleton)]
    public class SessionManager : IStateManager
    {
        private const string Prefix = "SM-";

        public void Set<T>(string key, T value)
        {
            key = $"{Prefix}{key}";
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[key] = value;
            }
            else
            {
                Log.Warn($"Session not ready. Could not set {key} to {value}", this);
            }
        }

        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            key = $"{Prefix}{key}";
            this.Set<T>(key, value);
        }

        public void Set<T>(string key, T value, bool isCacheable)
        {
            if (isCacheable)
            {
                this.Set(key, value);
            }
        }

        void IStateManager.Remove(string key)
        {
            key = $"{Prefix}{key}";
            HttpContext.Current.Session?.Remove(key);
        }

        public T Get<T>(string key)
        {
            key = $"{Prefix}{key}";
            object obj = HttpContext.Current.Session?[key];
            return obj == null ? default(T) : (T)obj;
        }

        public bool Exists(string key)
        {
            key = $"{Prefix}{key}";
            return HttpContext.Current.Session?[key] != null;
        }

        public void Clear()
        {
            if (HttpContext.Current.Session != null)
            {
                var keysToDelete = new List<string>();

                // first collect keys to delete (can't loop over keys while deleting)
                foreach (var key in HttpContext.Current.Session.Keys)
                {
                    var sKey = key?.ToString();
                    if (sKey != null && sKey.StartsWith(Prefix))
                    {
                        keysToDelete.Add(sKey);
                    }
                }

                // then delete
                foreach (var key in keysToDelete)
                {
                    HttpContext.Current.Session.Remove(key);
                }
            }
        }
    }
}