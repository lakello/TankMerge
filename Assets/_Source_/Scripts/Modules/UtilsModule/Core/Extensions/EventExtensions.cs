namespace _GameResources.Scripts.Modules.UtilsModule.Core.Extensions
{
	using System;
	using System.Threading;
	using Cysharp.Threading.Tasks;

	public static class EventExtensions
	{
		public static UniTask WaitForEventAsync(
			this object _,
			Action<Action> subscribe,
			Action<Action> unsubscribe,
			CancellationToken cancellationToken = default)
		{
			var tcs = new UniTaskCompletionSource();

			subscribe(Handler);

			var reg = cancellationToken.Register(() =>
			{
				unsubscribe(Handler);
				tcs.TrySetCanceled(cancellationToken);
			});

			if (cancellationToken.IsCancellationRequested)
			{
				reg.Dispose();
				unsubscribe(Handler);
				tcs.TrySetCanceled(cancellationToken);
			}

			return tcs.Task;
			
			void Handler()
			{
				tcs.TrySetResult();
			}
		}

		public static UniTask<T> WaitForEventAsync<T>(
			this object _,
			Action<Action<T>> subscribe,
			Action<Action<T>> unsubscribe,
			CancellationToken cancellationToken = default)
		{
			var tcs = new UniTaskCompletionSource<T>();

			subscribe(Handler);

			var reg = cancellationToken.Register(() =>
			{
				unsubscribe(Handler);
				tcs.TrySetCanceled(cancellationToken);
			});

			if (cancellationToken.IsCancellationRequested)
			{
				reg.Dispose();
				unsubscribe(Handler);
				tcs.TrySetCanceled(cancellationToken);
			}

			return tcs.Task;
			
			void Handler(T value)
			{
				tcs.TrySetResult(value);
			}
		}
		
		public static UniTask<(T1, T2)> WaitForEventAsync<T1, T2>(
			this object _,
			Action<Action<T1, T2>> subscribe,
			Action<Action<T1, T2>> unsubscribe,
			CancellationToken cancellationToken = default)
		{
			var tcs = new UniTaskCompletionSource<(T1, T2)>();

			subscribe(Handler);

			var reg = cancellationToken.Register(() =>
			{
				unsubscribe(Handler);
				tcs.TrySetCanceled(cancellationToken);
			});

			if (cancellationToken.IsCancellationRequested)
			{
				reg.Dispose();
				unsubscribe(Handler);
				tcs.TrySetCanceled(cancellationToken);
			}

			return tcs.Task;
			
			void Handler(T1 value1, T2 value2)
			{
				tcs.TrySetResult((value1, value2));
			}
		}
	}
}