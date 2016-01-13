using System;
using System.Collections.Concurrent;
using MSSQLServerAuditor.Gui;

namespace MSSQLServerAuditor.Managers
{
	public class TreeTaskThreadCounter
	{
		private static readonly ConcurrentDictionary<int, int> ConnectionTaskCounters;
		private readonly int                                   _connectionHashCode;

		static TreeTaskThreadCounter()
		{
			ConnectionTaskCounters = new ConcurrentDictionary<int, int>();
		}

		public TreeTaskThreadCounter(ConnectionData connection)
		{
			this._connectionHashCode = connection.GetHashCode();
		}

		public bool CanAdd
		{
			get
			{
				int maxThreads = Program.Model.Settings.MaximumDBRequestsThreadCount;

				if (maxThreads == 0)
				{
					return true;
				}

				return ConnectionTaskCounters.GetOrAdd(
					this._connectionHashCode, 0) < maxThreads;
			}
		}

		public void Add(int count)
		{
			ConnectionTaskCounters.AddOrUpdate(
				this._connectionHashCode,
				count,
				(key, val) => val + count
			);
		}

		public void Increment()
		{
			Add(1);
		}

		public void Decrement()
		{
			Remove(1);
		}

		public void Remove(int count)
		{
			if (!ConnectionTaskCounters.ContainsKey(this._connectionHashCode))
			{
				throw new ArgumentOutOfRangeException("connection");
			}

			ConnectionTaskCounters[this._connectionHashCode] -= count;
		}
	}
}
