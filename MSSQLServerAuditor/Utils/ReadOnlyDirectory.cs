using System;
using System.Collections;
using System.Collections.Generic;

namespace MSSQLServerAuditor.Utils
{
	/// <summary>
	/// Read-only wrapper around dictionary.
	/// </summary>
	public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
	{
		private readonly IDictionary<TKey, TValue> _wrapped;

		public ReadOnlyDictionary(IDictionary<TKey, TValue> wrapped)
		{
			this._wrapped = wrapped;
		}

		public void Add(TKey key, TValue value)
		{
			throw new InvalidOperationException();
		}

		public bool ContainsKey(TKey key)
		{
			return this._wrapped.ContainsKey(key);
		}

		public ICollection<TKey> Keys
		{
			get { return this._wrapped.Keys; }
		}

		public bool Remove(TKey key)
		{
			throw new InvalidOperationException();
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return this._wrapped.TryGetValue(key, out value);
		}

		public ICollection<TValue> Values
		{
			get { return this._wrapped.Values; }
		}

		public TValue this[TKey key]
		{
			get { return this._wrapped[key]; }
			set { throw new InvalidOperationException(); }
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			throw new InvalidOperationException();
		}

		public void Clear()
		{
			throw new InvalidOperationException();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return this._wrapped.Contains(item);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this._wrapped.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return this._wrapped.Count; }
		}

		public bool IsReadOnly
		{
			get { return true; }
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			throw new InvalidOperationException();
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this._wrapped.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this._wrapped).GetEnumerator();
		}

		public override bool Equals(object obj)
		{
			return this._wrapped.Equals(obj);
		}

		public override int GetHashCode()
		{
			return this._wrapped.GetHashCode();
		}

		public override string ToString()
		{
			return this._wrapped.ToString();
		}
	}
}
