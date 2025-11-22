namespace UtilsModule.Other
{
	using System.Threading;
	using System;
	using System.Collections.Generic;
	using Cysharp.Threading.Tasks;
	using UnityEngine;

	public class DContainer
	{
		private readonly Dictionary<Type, object> _data = new Dictionary<Type, object>();
		private readonly string _name;
		
		public void Show()
		{
			foreach (var key in _data.Keys)
			{
				Debug.LogWarning($"{key}");
			}
		}

		public bool Contains<T>()
		{
			return _data.ContainsKey(typeof(T));
		}

		public ContractHolder Replace<T>(T instance)
		{
			if (Contains<T>())
			{
				Release<T>();
			}

			return Register(instance);
		}

		public ContractHolder Register<T>(T instance)
		{
			return RegisterByType<T>(instance);
		}
		
		public ContractHolder ReplaceByType<T>(object instance)
		{
			if (Contains<T>())
			{
				Release<T>();
			}

			return RegisterByType<T>(instance);
		}

		public ContractHolder RegisterByType<T>(object instance)
		{
			if (_data.ContainsKey(typeof(T)))
			{
				throw new ArgumentException($"The type {typeof(T)} already registered.");
			}

			_data[typeof(T)] = instance;

			var disposable = new ContractHolder(
				this,
				instance,
				() =>
				{
					if (_data.TryGetValue(typeof(T), out var value))
					{
						_data.Remove(typeof(T));

						if (value is IDisposable disposable)
						{
							disposable.Dispose();
						}
					}
				});

			return disposable;
		}

		public T Resolve<T>()
		{
			if (_data.ContainsKey(typeof(T)))
			{
				return (T)_data[typeof(T)];
			}

			throw new ArgumentException($"The type {typeof(T)} does not registered.");
		}

		public bool TryResolve<T>(out T result)
		{
			if (_data.ContainsKey(typeof(T)))
			{
				result = (T)_data[typeof(T)];
				return true;
			}

			result = default(T);
			return false;
		}

		public void Resolve<T>(ref T instance)
		{
			instance = Resolve<T>();
		}

		public async UniTask<T> ResolveAsync<T>(CancellationToken token = default)
		{
			await UniTask.WaitUntil(() => _data.ContainsKey(typeof(T)), cancellationToken: token);
			token.ThrowIfCancellationRequested();
			return (T)_data[typeof(T)];
		}

		public void Release<T>()
		{
			if (_data.ContainsKey(typeof(T)))
			{
				_data.Remove(typeof(T));
			}
		}

		public void Dispose()
		{
			foreach (var v in _data.Values)
			{
				if (v is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}

			_data.Clear();
		}

		public class ContractHolder : IDisposable
		{
			private readonly DContainer container;
			private readonly object _obj;
			private readonly Action _dispose;

			public ContractHolder(DContainer container, object obj, Action dispose)
			{
				this.container = container;
				_obj = obj;
				_dispose = dispose;
			}

			public ContractHolder As<T>()
			{
				return container.RegisterByType<T>(_obj);
			}

			public void Dispose()
			{
				_dispose();
			}
		}
	}
}