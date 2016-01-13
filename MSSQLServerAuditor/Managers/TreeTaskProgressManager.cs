using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using log4net;

namespace MSSQLServerAuditor.Managers
{
	public class TreeTaskProgressManager
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public event EventHandler                         Changed;
		public event EventHandler                         EverythingIsDone;
		public readonly ConcurrentDictionary<int, string> Waiting;
		public readonly ConcurrentDictionary<int, string> Running;
		public bool                                       IsCancelRequested;

		public TreeTaskProgressManager()
		{
			this.Waiting           = new ConcurrentDictionary<int, string>();
			this.Running           = new ConcurrentDictionary<int, string>();
			this.IsCancelRequested = false;
		}

		public TreeTaskProgressManager(TreeTaskManager taskManager) : this()
		{
			taskManager.RunningTasks.CollectionChanged += RunningTasksOnCollectionChanged;
		}

		private void RunningTasksOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (args.NewItems != null)
			{
				foreach (KeyValuePair<long, TreeTask> ti in args.NewItems)
				{
					TreeTask treeTask = ti.Value;

					treeTask.JobChanged += (s, e) =>
					{
						string val = null;

						switch (e.Event)
						{
							case TreeJobChangedEvent.WaitingAdded:
								this.Waiting.TryAdd(e.Job.GetHashCode(), e.Job.Title);
								break;

							case TreeJobChangedEvent.WaitingScheduled:
								this.Waiting.TryRemove(e.Job.GetHashCode(), out val);
								break;

							case TreeJobChangedEvent.RunningAdded:
								this.Running.TryAdd(e.Job.GetHashCode(), e.Job.Title);
								break;

							case TreeJobChangedEvent.RunningCompleted:
								this.Running.TryRemove(e.Job.GetHashCode(), out val);
								break;

							default:
								throw new ArgumentOutOfRangeException();
						}

						RaiseOnChanged();
					};

					treeTask.Canceled += (o, eventArgs) =>
					{
						this.IsCancelRequested = true;
						this.Waiting.Clear();
						RaiseOnChanged();
					};
				}
			}
		}

		public void RaiseOnChanged()
		{
			if (Changed != null)
			{
				Changed(this, EventArgs.Empty);
			}

			if (this.Running.Count == 0 && this.Waiting.Count == 0)
			{
				IsCancelRequested = false;

				if (EverythingIsDone != null)
				{
					EverythingIsDone(this, EventArgs.Empty);
				}
			}
		}
	}
}
