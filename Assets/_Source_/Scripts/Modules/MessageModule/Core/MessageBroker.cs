namespace MessageModule
{
	using System;
	using System.Collections.Generic;
	using R3;
	using UnityEngine;

	public interface IMessageBroker
	{
		public void Show();

		void Publish<T>(T message);

		Observable<T> Receive<T>();
	}

	public sealed class MessageBroker : IMessageBroker, IDisposable
	{
		private readonly Dictionary<Type, object> _notifiers = new Dictionary<Type, object>();

		private bool _isDisposed;

		public static IMessageBroker Default { get; } = new MessageBroker();

		public void Show()
		{
			Debug.Log("SHOW BROKER");
			
			var keys = _notifiers.Keys;

			foreach (var key in keys)
			{
				Debug.Log($"KEY = {key.Name} ||| Value: {_notifiers[key].GetType().Name}");
			}
		}

		public void Publish<T>(T message)
		{
			object notifier;

			lock (_notifiers)
			{
				if (_isDisposed) return;

				if (!_notifiers.TryGetValue(typeof(T), out notifier))
				{
					return;
				}
			}

			((Subject<T>)notifier).OnNext(message);
		}

		public Observable<T> Receive<T>()
		{
			object notifier;

			lock (_notifiers)
			{
				if (_isDisposed)
				{
					throw new ObjectDisposedException("MessageBroker");
				}

				if (!_notifiers.TryGetValue(typeof(T), out notifier))
				{
					Subject<T> n = new Subject<T>();
					notifier = n;
					_notifiers.Add(typeof(T), notifier);
				}
			}

			return (Observable<T>)notifier;
		}

		public void Dispose()
		{
			lock (_notifiers)
			{
				if (!_isDisposed)
				{
					_isDisposed = true;
					_notifiers.Clear();
				}
			}
		}
	}
}