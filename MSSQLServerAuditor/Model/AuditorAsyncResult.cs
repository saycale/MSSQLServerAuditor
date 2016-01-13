using System;
using System.Threading;

namespace MSSQLServerAuditor.Model
{
	internal class AuditorAsyncResult<T> : IAsyncResult
	{
		private T              _data;
		private object         _state;
		private bool           _isCompleted;
		private AutoResetEvent _waitHandle;
		private bool           _isSynchronous;

		public T Data
		{
			set { this._data = value; }
			get { return this._data; }
		}

		public AuditorAsyncResult()
		{
			this._state         = null;
			this._isCompleted   = false;
			this._waitHandle    = null;
			this._isSynchronous = false;
		}

		public AuditorAsyncResult(bool synchronous, object stateData) : this()
		{
			this._isSynchronous = synchronous;
			this._state         = stateData;
		}

		public void Complete()
		{
			this._isCompleted = true;
			((AutoResetEvent)AsyncWaitHandle).Set();
		}

		public object AsyncState
		{
			get { return this._state; }
		}

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				if (this._waitHandle == null)
				{
					this._waitHandle = new AutoResetEvent(false);
				}

				return this._waitHandle;
			}
		}

		public bool CompletedSynchronously
		{
			get
			{
				if (!this._isCompleted)
				{
					return false;
				}
				else
				{
					return this._isSynchronous;
				}
			}
		}

		public bool IsCompleted
		{
			get { return this._isCompleted; }
		}
	}
}
