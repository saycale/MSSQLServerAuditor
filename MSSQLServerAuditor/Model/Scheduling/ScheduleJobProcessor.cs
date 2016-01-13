using System;
using System.Threading.Tasks;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.Model.Scheduling
{
	public abstract class AbstractJobProcessor
	{
		public event EventHandler<JobEventArgs> JobStarted;

		public void RunIfTime(
			Job       job,
			DateTime  now,
			Action    action = null,
			DateTime? minPossibleTime = null
		)
		{
			if (IsTimeToRun(job, now, minPossibleTime))
			{
				if (action == null)
				{
					Run(job, now, () => { });
				}
				else
				{
					Run(job, now, action);
				}
			}
		}

		public void RunIfTime(
			Job       job,
			Action    action = null,
			DateTime? minPossibleTime = null
		)
		{
			DateTime now = DateTime.Now;

			RunIfTime(job, now, action, minPossibleTime);
		}

		public bool IsTimeToRun(
			Job job,
			DateTime now,
			DateTime? minPossibleTime = null
		)
		{
			DateTime searchOrigin = job.LastRan ?? job.Settings.StartDate.Subtract(new TimeSpan(1));

			if (minPossibleTime != null && minPossibleTime > searchOrigin)
			{
				searchOrigin = minPossibleTime.Value;
			}

			if (now < searchOrigin)
			{
				return false;
			}

			DateTime nextAfterLast;

			if (!job.Settings.GetNextTime(searchOrigin, out nextAfterLast))
			{
				return false;
			}

			if (nextAfterLast > now)
			{
				return false;
			}

			return true;
		}

		private void Run(
			Job      job,
			DateTime executeTime,
			Action   action)
		{
			RunJob(job, executeTime, action);
			OnJobStarted(job, executeTime);
		}

		private void OnJobStarted(
			Job      job,
			DateTime executeTime
		)
		{
			var handler = JobStarted;

			if (handler != null)
			{
				handler(this, new JobEventArgs(job, executeTime));
			}
		}

		protected abstract void RunJob(
			Job      job,
			DateTime executeTime,
			Action   action
		);
	}

	public class JobEventArgs : EventArgs
	{
		public JobEventArgs(Job job, DateTime dateTime)
		{
			this.Job      = job;
			this.DateTime = dateTime;
		}

		public DateTime DateTime { get; private set; }
		public Job      Job      { get; private set; }
	}

	public class SimpleJobProcessor : AbstractJobProcessor
	{
		protected override void RunJob(Job job, DateTime executeTime, Action action)
		{
			action();

			job.LastRan = executeTime;
		}
	}

	public class ScheduleJobProcessor : AbstractJobProcessor
	{
		private readonly CurrentStorage _storage;

		public ScheduleJobProcessor(CurrentStorage storage)
		{
			this._storage = storage;
		}

		protected override void RunJob(
			Job      job,
			DateTime executeTime,
			Action   action
		)
		{
			long settingsId = job.SettingsId;

			Task.Factory.StartNew(() =>
			{
				this._storage.ScheduleSettingsTable.SaveLastRun(
					settingsId,
					executeTime
				);

				action();

				job.LastRan = executeTime;

				this._storage.NodeInstances.SaveScheduledUpdateTime(
					job.NodeInfo,
					DateTime.Now
				);
			});
		}
	}
}
