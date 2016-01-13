using System;

namespace MSSQLServerAuditor.Managers
{
	public class TreeTaskProgressInfo
	{
		public event EventHandler<TreeJobProgressChangedEventArgs> ProgressChanged;

		public float MaxValue = 100.0F;

		public float ItemsCount { get; private set; }

		private float progressValue;

		public float ProgressValue
		{
			get { return progressValue; }

			private set
			{
				if (progressValue.Equals(value))
				{
					return;
				}

				progressValue = value;

				if(ProgressChanged != null)
				{
					ProgressChanged(this, new TreeJobProgressChangedEventArgs(progressValue));
				}
			}
		}

		public void AddItems(int count)
		{
			this.ItemsCount += (float)count;
		}

		public void CompleteItems(int count)
		{
			ProgressValue += (MaxValue - progressValue) * (float)count / ItemsCount;
		}

		public void SetDone()
		{
			ProgressValue = MaxValue;
		}

		public void SetCanceled()
		{
			this.ProgressValue = 0.0F;
		}
	}
}
