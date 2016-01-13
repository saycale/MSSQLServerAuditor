using System;
using System.Collections.Generic;
using System.Linq;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	/// Progress change Event args
	/// </summary>
	public class ProgressChangedEventArgs : EventArgs
	{
	}

	/// <summary>
	/// Delegate for progerss change
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event args</param>
	public delegate void ProgressChangedDelegate(object sender, ProgressChangedEventArgs e);

	/// <summary>
	/// Progress item
	/// </summary>
	public class ProgressItem
	{
		readonly object             _globalLock;
		private double              _currentValue;
		readonly List<ProgressItem> _subItems;

		public ProgressItem()
		{
			this._globalLock   = new object();
			this._currentValue = 0.0;
			this._subItems     = new List<ProgressItem>();
		}

		/// <summary>
		/// Progress changed
		/// </summary>
		public event ProgressChangedDelegate ProgressChanged;

		/// <summary>
		/// Occurs in case of progress change.
		/// </summary>
		/// <param name="e">ProgressChangedEventArgs.</param>
		protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
		{
			ProgressChangedDelegate handler = ProgressChanged;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Current progress value
		/// </summary>
		public double Value
		{
			get
			{
				lock (this._globalLock)
				{
					if (this._subItems.Count == 0)
					{
						return this._currentValue;
					}

					return this._subItems.Sum(i => i.Value) / this._subItems.Count;
					//  return _subItems.Sum(i => i.Value)/_promisedChildCount;
				}
			}
		}

		/// <summary>
		/// Maximum value.
		/// </summary>
		public double MaxValue { get { return 100; } }

		/// <summary>
		/// Set current progress (can't be called if child progress items exists).
		/// </summary>
		/// <param name="value">Value from 0 to 100</param>
		public void SetProgress(double value)
		{
			lock (this._globalLock)
			{
				if (this._subItems.Count > 0)
				{
					throw  new InvalidOperationException("Trying to set value for progress with childs.");
				}

				this._currentValue = value;
			}

			OnProgressChanged(new ProgressChangedEventArgs());
		}

		/// <summary>
		/// To set the progress end.
		/// </summary>
		public void SetProgressDone()
		{
			SetProgress(MaxValue);
		}

		/// <summary>
		/// Set progress canceled.
		/// </summary>
		public void SetProgressCanceled()
		{
			SetProgress(0);
		}

		/// <summary>
		/// Set count of child progress items will be requested
		/// </summary>
		/// <param name="count">Count</param>
		public void SetPromisedChildCount(int count)
		{
			// lock (this._globalLock)
			// _promisedChildCount = count;
		}

		/// <summary>
		/// Get child progress item (before it's count should be set through SetPromisedChildCount).
		/// </summary>
		/// <returns>Child progress item</returns>
		public ProgressItem GetChild()
		{
			lock (this._globalLock)
			{
				// if (this._subItems.Count >= _promisedChildCount)
				//     throw new InvalidOperationException("Trying to get more children that was promised");

				var result = new ProgressItem();
				result.ProgressChanged += (sender, args) => OnProgressChanged(args);

				this._subItems.Add(result);

				return result;
			}
		}
	}
}
