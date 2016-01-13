using System;
using System.Threading;

namespace MSSQLServerAuditor.Model
{
	internal class AsyncResult<T> : IAsyncResult, IDisposable
	{
		private readonly AsyncCallback    _callback;
		private readonly object           _asyncState;
		private readonly ManualResetEvent _waitHandle;
		private readonly object           _syncRoot;
		private bool                      _completed;
		private bool                      _completedSynchronously;
		private T                         _result;
		private Exception                 _exception;

		internal AsyncResult(
			AsyncCallback callback,
			object        state,
			bool          completed = false
		)
		{
			this._callback               = callback;
			this._asyncState             = state;
			this._completed              = completed;
			this._completedSynchronously = completed;
			this._waitHandle             = new ManualResetEvent(false);
			this._syncRoot               = new object();
		}

		#region IAsyncResult Members

		public object AsyncState
		{
			get { return this._asyncState; }
		}

		public WaitHandle AsyncWaitHandle
		{
			get { return this._waitHandle; }
		}

		public bool CompletedSynchronously
		{
			get
			{
				lock (this._syncRoot)
				{
					return this._completedSynchronously;
				}
			}
		}

		public bool IsCompleted
		{
			get
			{
				lock (this._syncRoot)
				{
					return this._completed;
				}
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				lock (this._syncRoot)
				{
					if (this._waitHandle != null)
					{
						((IDisposable)this._waitHandle).Dispose();
					}
				}
			}
		}

		internal Exception Exception
		{
			get
			{
				lock (this._syncRoot)
				{
					return this._exception;
				}
			}
		}

		internal T Result
		{
			get
			{
				lock (this._syncRoot)
				{
					return this._result;
				}
			}
		}

		internal void Complete(
			T    result,
			bool completedSynchronously
		)
		{
			lock (this._syncRoot)
			{
				this._completed              = true;
				this._completedSynchronously = completedSynchronously;
				this._result                 = result;
			}

			this.SignalCompletion();
		}

		internal void HandleException(
			Exception exception,
			bool      completedSynchronously
		)
		{
			lock (this._syncRoot)
			{
				this._completed              = true;
				this._completedSynchronously = completedSynchronously;
				this._exception              = exception;
			}

			this.SignalCompletion();
		}

		private void SignalCompletion()
		{
			this._waitHandle.Set();

			ThreadPool.QueueUserWorkItem(new WaitCallback(this.InvokeCallback));
		}

		private void InvokeCallback(object state)
		{
			if (this._callback != null)
			{
				this._callback(this);
			}
		}
	}
}
