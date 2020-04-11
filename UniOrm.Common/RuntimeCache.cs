using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Common
{
    public class RuntimeCache
    {
        public TItem Set<TItem>( object key, TItem value)
        {
            return Cache.Set(key, value);
        }
        public TItem GetOrCreate<TItem>(  object key, Func<ICacheEntry, TItem> factory)
        {
            if(!keys.Contains(key))
            {
                keys.Add(key);
            }
            return Cache.GetOrCreate(key, factory);
        }
        List<object> keys = new List<object>();
        public IMemoryCache Cache { get; set; }
        public RuntimeCache(IMemoryCache cache)
        {
            Cache = cache;
        }

         public void Clear()
        {
            foreach (var k in keys)
            {
                try
                {
                    Cache.Remove(k);

                }
                catch
                { }
            }
            keys.Clear();
        }
    }
}
