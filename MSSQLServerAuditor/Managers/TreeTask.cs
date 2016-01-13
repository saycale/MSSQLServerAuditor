using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Managers
{
	public class TreeTask
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public event EventHandler<TreeJobChangedEventArgs>   JobChanged;
		public event EventHandler<TreeJobCompletedEventArgs> JobCompleted;
		public event EventHandler                            Completed;
		public event EventHandler                            Canceled;
		private readonly ConcurrentQueue<TreeJob>            _waitingJobs;
		private readonly ObservableCollection<TreeJob>       _runningJobs;
		private readonly object                              _runningJobsLock;
		private readonly TreeTaskThreadCounter               _threadCounter;

		public TreeTaskProgressInfo    Progress           { get; private set; }
		public CancellationTokenSource CancellationSource { get; private set; }
		public TreeTaskInfo            Info               { get; private set; }

		public static TreeTask Create(TreeTaskInfo taskInfo)
		{
			return new TreeTask(taskInfo);
		}

		public TreeTask(TreeTaskInfo taskInfo)
		{
			this.Info               = taskInfo;
			this._threadCounter     = new TreeTaskThreadCounter(taskInfo.Connection);
			this._waitingJobs       = new ConcurrentQueue<TreeJob>();
			this._runningJobs       = new ObservableCollection<TreeJob>();
			this._runningJobsLock   = new object();
			this.CancellationSource = new CancellationTokenSource();
			this.Progress           = new TreeTaskProgressInfo();

			Progress.AddItems(1);
		}

		public void Cancel()
		{
			this.CancellationSource.Cancel();
			this._waitingJobs.Clear();

			if (this.Canceled != null)
			{
				Canceled(this, EventArgs.Empty);
			}
		}

		public void Schedule(TreeJob treeJob)
		{
			if (this.CancellationSource.IsCancellationRequested)
			{
				return;
			}

			if (this._threadCounter.CanAdd)
			{
				Execute(treeJob);
			}
			else
			{
				if (!this.CancellationSource.IsCancellationRequested)
				{
					this._waitingJobs.Enqueue(treeJob);
					OnJobChanged(treeJob, TreeJobChangedEvent.WaitingAdded);
				}
			}
		}

		public bool SubmitNextJob()
		{
			if (!this._threadCounter.CanAdd)
			{
				return false;
			}

			TreeJob waitingJob;

			if (this._waitingJobs.TryDequeue(out waitingJob))
			{
				OnJobChanged(waitingJob, TreeJobChangedEvent.WaitingScheduled);
				Execute(waitingJob);

				return true;
			}

			return false;
		}

		private void Execute(TreeJob treeJob)
		{
			lock (this._runningJobsLock)
			{
				this._threadCounter.Increment();

				treeJob.State = TreeJobState.Running;
				this._runningJobs.Add(treeJob);
				OnJobChanged(treeJob, TreeJobChangedEvent.RunningAdded);
			}

			Task.Factory.StartNew(treeJob.Action)
				.ContinueWith(t =>
				{
					lock (this._runningJobsLock)
					{
						this._runningJobs.Remove(treeJob);
					}

					treeJob.State = TreeJobState.Completed;
					OnJobCompleted(treeJob);

					Progress.AddItems(treeJob.PromisedChildCount);

					Progress.CompleteItems(1);
					this._threadCounter.Decrement();

					OnJobChanged(treeJob, TreeJobChangedEvent.RunningCompleted);

					CheckOnCompleted();
				})
				.ContinueOnException(t =>
				{
					if (t.Exception != null)
					{
						log.ErrorFormat("Error occurred while processing task completion: {0}", t.Exception);
					}
				});
		}

		private void CheckOnCompleted()
		{
			if (this._runningJobs.Count == 0 && this._waitingJobs.Count == 0)
			{
				if (Completed != null)
				{
					Completed(this, EventArgs.Empty);
				}
			}
		}

		private void OnJobCompleted(TreeJob treeJob)
		{
			if (CheckOnJobCompleted(treeJob))
			{
				if (JobCompleted != null)
				{
					JobCompleted(this, new TreeJobCompletedEventArgs(treeJob));
				}
			}
		}

		private bool CheckOnJobCompleted(TreeJob treeJob)
		{
			if (treeJob.State != TreeJobState.Completed)
			{
				return false;
			}

			bool childrenCompleted = true;

			foreach (TreeJob childJob in treeJob.ChildJobs)
			{
				bool childCompleted = CheckOnJobCompleted(childJob);

				if (childCompleted)
				{
					treeJob.RemoveChildJob(childJob);
				}

				childrenCompleted &= childCompleted;
			}

			if (childrenCompleted)
			{
				TreeJob parentJob = treeJob.Parent;

				if (parentJob != null)
				{
					parentJob.RemoveChildJob(treeJob);
					OnJobCompleted(parentJob);
				}
			}

			return childrenCompleted;
		}

		private void OnJobChanged(TreeJob treeJob, TreeJobChangedEvent args)
		{
			if (JobChanged != null)
			{
				JobChanged(this, new TreeJobChangedEventArgs(treeJob, args));
			}
		}
	}
}
