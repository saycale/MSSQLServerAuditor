using System;
using System.Threading.Tasks;

namespace MSSQLServerAuditor.Utils
{
	public static class TaskContinuationExtensions
	{
		/// <summary>
		/// Run the 2nd task <paramref name="continueWith"/>
		/// when <paramref name="task"/> is not finished because of any exception. Then
		/// run the (optional) 3rd task <paramref name="onFinished"/>
		/// by using <see cref="TaskScheduler.FromCurrentSynchronizationContext()"/>.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="task"></param>
		/// <param name="continueWith"></param>
		/// <param name="onFinished"></param>
		/// <returns></returns>
		public static Task ContinueOnException<TResult>(
			this Task<TResult> task,
			Action<Task<TResult>> continueWith,
			Action<Task> onFinished = null)
		{
			return task.ContinueOn(continueWith, TaskContinuationOptions.OnlyOnFaulted, onFinished);
		}

		/// <summary>
		/// Run the 2nd task <paramref name="continueWith"/>
		/// when <paramref name="task"/> is not finished because of any exception. Then
		/// run the (optional) 3rd task <paramref name="onFinished"/>
		/// by using <see cref="TaskScheduler.FromCurrentSynchronizationContext()"/>.
		/// </summary>
		/// <param name="task"></param>
		/// <param name="continueWith"></param>
		/// <param name="onFinished"></param>
		/// <returns></returns>
		public static Task ContinueOnException(
			this Task task,
			Action<Task> continueWith,
			Action<Task> onFinished = null)
		{
			return task.ContinueOn(continueWith, TaskContinuationOptions.OnlyOnFaulted, onFinished);
		}

		/// <summary>
		/// Run the 2nd task <paramref name="continueWith"/>
		/// when <paramref name="task"/> is canceled. Then
		/// run the (optional) 3rd task <paramref name="onFinished"/>
		/// by using <see cref="TaskScheduler.FromCurrentSynchronizationContext()"/>.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="task"></param>
		/// <param name="continueWith"></param>
		/// <param name="onFinished"></param>
		/// <returns></returns>
		public static Task ContinueOnCancelled<TResult>(
			this Task<TResult> task,
			Action<Task<TResult>> continueWith,
			Action<Task> onFinished = null)
		{
			return task.ContinueOn(continueWith, TaskContinuationOptions.OnlyOnCanceled, onFinished);
		}

		/// <summary>
		/// Run the 2nd task <paramref name="continueWith"/>
		/// when <paramref name="task"/> is canceled. Then
		/// run the (optional) 3rd task <paramref name="onFinished"/>
		/// by using <see cref="TaskScheduler.FromCurrentSynchronizationContext()"/>.
		/// </summary>
		/// <param name="task"></param>
		/// <param name="continueWith"></param>
		/// <param name="onFinished"></param>
		/// <returns></returns>
		public static Task ContinueOnCancelled
			(this Task task,
			Action<Task> continueWith,
			Action<Task> onFinished = null)
		{
			return task.ContinueOn(continueWith, TaskContinuationOptions.OnlyOnCanceled, onFinished);
		}

		/// <summary>
		/// Run the 2nd task <paramref name="continueWith"/>
		/// when <paramref name="task"/> is finished successfully. Then
		/// run the (optional) 3rd task <paramref name="onFinished"/>
		/// by using <see cref="TaskScheduler.FromCurrentSynchronizationContext()"/>.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="task"></param>
		/// <param name="continueWith"></param>
		/// <param name="onFinished"></param>
		/// <returns></returns>
		public static Task ContinueOnCompleted<TResult>(
			this Task<TResult> task,
			Action<Task<TResult>> continueWith,
			Action<Task> onFinished = null)
		{
			return task.ContinueOn(continueWith, TaskContinuationOptions.OnlyOnRanToCompletion, onFinished);
		}

		/// <summary>
		/// Run the 2nd task <paramref name="continueWith"/>
		/// when <paramref name="task"/> is finished successfully. Then
		/// run the (optional) 3rd task <paramref name="onFinished"/>
		/// by using <see cref="TaskScheduler.FromCurrentSynchronizationContext()"/>.
		/// </summary>
		/// <param name="task"></param>
		/// <param name="continueWith"></param>
		/// <param name="onFinished"></param>
		/// <returns></returns>
		public static Task ContinueOnCompleted(
			this Task task,
			Action<Task> continueWith,
			Action<Task> onFinished = null)
		{
			return task.ContinueOn(continueWith, TaskContinuationOptions.OnlyOnRanToCompletion, onFinished);
		}

		private static Task ContinueOn<TResult>(
			this Task<TResult> task,
			Action<Task<TResult>> continueWith,
			TaskContinuationOptions option,
			Action<Task> onFinished)
		{
			if (onFinished == null)
			{
				return task.ContinueWith(continueWith, option);
			}

			return task.ContinueWith(continueWith, option)
				.ContinueWith(onFinished, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private static Task ContinueOn(
			this Task task,
			Action<Task> continueWith,
			TaskContinuationOptions option,
			Action<Task> onFinished)
		{
			if (onFinished == null)
			{
				return task.ContinueWith(continueWith, option);
			}

			return task.ContinueWith(continueWith, option)
				.ContinueWith(onFinished, TaskScheduler.FromCurrentSynchronizationContext());
		}
	}
}
