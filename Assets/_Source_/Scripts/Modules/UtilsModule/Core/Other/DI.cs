namespace UtilsModule.Other
{
	using System.Threading;
	using System;
	using System.Collections.Generic;
	using Cysharp.Threading.Tasks;
	using UnityEngine;

	public static class DI
	{
		private static Dictionary<Type, object> s_data = new Dictionary<Type, object>();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			s_data = new Dictionary<Type, object>();
		}

		public static void Show()
		{
			foreach (var key in s_data.Keys)
			{
				Debug.LogWarning($"{key}");
			}
		}

		public static bool Contains<T>()
		{
			return s_data.ContainsKey(typeof(T));
		}

		public static ContractHolder Replace<T>(T instance)
		{
			if (Contains<T>())
			{
				Release<T>();
			}

			return Register(instance);
		}

		public static ContractHolder Register<T>(T instance)
		{
			return RegisterByType<T>(instance);
		}
		
		public static ContractHolder ReplaceByType<T>(object instance)
		{
			if (Contains<T>())
			{
				Release<T>();
			}

			return RegisterByType<T>(instance);
		}

		public static ContractHolder RegisterByType<T>(object instance)
		{
			if (s_data.ContainsKey(typeof(T)))
			{
				throw new ArgumentException($"The type {typeof(T)} already registered.");
			}

			s_data[typeof(T)] = instance;

			var disposable = new ContractHolder(
				instance,
				() =>
				{
					if (s_data.TryGetValue(typeof(T), out var value))
					{
						s_data.Remove(typeof(T));

						if (value is IDisposable disposable)
						{
							disposable.Dispose();
						}
					}
				});

			return disposable;
		}

		public static T Resolve<T>()
		{
			if (s_data.ContainsKey(typeof(T)))
			{
				return (T)s_data[typeof(T)];
			}

			throw new ArgumentException($"The type {typeof(T)} does not registered.");
		}

		public static bool TryResolve<T>(out T result)
		{
			if (s_data.ContainsKey(typeof(T)))
			{
				result = (T)s_data[typeof(T)];
				return true;
			}

			result = default(T);
			return false;
		}

		public static void Resolve<T>(ref T instance)
		{
			instance = Resolve<T>();
		}

		public static async UniTask<T> ResolveAsync<T>(CancellationToken token = default)
		{
			await UniTask.WaitUntil(() => s_data.ContainsKey(typeof(T)), cancellationToken: token);
			token.ThrowIfCancellationRequested();
			return (T)s_data[typeof(T)];
		}

		public static void Release<T>()
		{
			if (s_data.ContainsKey(typeof(T)))
			{
				s_data.Remove(typeof(T));
			}
		}

		public static void Dispose()
		{
			foreach (var v in s_data.Values)
			{
				if (v is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}

			s_data.Clear();
		}

		public class ContractHolder : IDisposable
		{
			private readonly object _obj;
			private readonly Action _dispose;

			public ContractHolder(object obj, Action dispose)
			{
				_obj = obj;
				_dispose = dispose;
			}

			public ContractHolder As<T>()
			{
				return DI.RegisterByType<T>(_obj);
			}

			public void Dispose()
			{
				_dispose();
			}
		}
	}
}