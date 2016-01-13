using System;
using System.Collections.Concurrent;

namespace MSSQLServerAuditor.Cache
{
	public class MemoryCache
	{
		private readonly ConcurrentDictionary<object, object> _cache;

		public MemoryCache()
		{
			this._cache = new ConcurrentDictionary<object, object>();
		}

		public bool Remove(string key)
		{
			object o            = null;
			bool   isKeyRemoved = false;

			if (this._cache.ContainsKey(key))
			{
				isKeyRemoved = this._cache.TryRemove(key, out o);
			}

			return isKeyRemoved;
		}

		public T GetOrAdd<T>(object key, Func<T> selector, T defaultValue = default (T))
		{
			if (this._cache.ContainsKey(key))
			{
				var cacheValue = this._cache[key];

				if (cacheValue is T)
				{
					return (T)cacheValue;
				}

				return defaultValue;
			}

			var value = selector();

			if (value == null)
			{
				return defaultValue;
			}

			this._cache[key] = value;

			return (T) this._cache[key];
		}
	}
}
