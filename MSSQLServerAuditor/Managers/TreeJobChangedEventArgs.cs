using System;

namespace MSSQLServerAuditor.Managers
{
	public enum TreeJobChangedEvent
	{
		WaitingAdded,
		WaitingScheduled,
		RunningAdded,
		RunningCompleted
	}

	public class TreeJobChangedEventArgs : EventArgs
	{
		public TreeJob             Job   { get; private set; }
		public TreeJobChangedEvent Event { get; private set; }

		public TreeJobChangedEventArgs(TreeJob job, TreeJobChangedEvent jobEvent)
		{
			this.Job   = job;
			this.Event = jobEvent;
		}
	}

	public class TreeJobCompletedEventArgs : EventArgs
	{
		public TreeJob Job { get; private set; }

		public TreeJobCompletedEventArgs(TreeJob job)
		{
			this.Job = job;
		}
	}

	public class TreeJobProgressChangedEventArgs : EventArgs
	{
		public float NewValue { get; set; }

		public TreeJobProgressChangedEventArgs(float newValue)
		{
			this.NewValue = newValue;
		}
	}
}
