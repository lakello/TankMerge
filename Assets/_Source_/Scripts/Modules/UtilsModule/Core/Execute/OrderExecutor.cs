namespace UtilsModule.Execute
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using AYellowpaper;
	using Cysharp.Threading.Tasks;
	using Extensions;
	using Interfaces;
	using Sirenix.OdinInspector;
	using UnityEngine;

	public class OrderExecutor : MonoBehaviour
	{
		[SerializeField]
		private InterfaceReference<IExecuteHolder>[] _awakeExecutors;
		[SerializeField]
		private InterfaceReference<IExecuteHolder>[] _startExecutors;

		private CancellationTokenSource _source;

		[Button]
		private void FindExecutors()
		{
			SetIndexes();

			var objects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);

			List<IExecuteHolder> executors = new List<IExecuteHolder>();

			foreach (var t in objects)
			{
				var e = t.GetComponents<IExecuteHolder>();

				if (e is { Length: > 0 })
				{
					executors.AddRange(e);
				}
			}

			_awakeExecutors = executors
				.Where(e => e.Method == ExecuteMethod.Awake)
				.OrderBy(e => e.Priority)
				.Select(e => new InterfaceReference<IExecuteHolder>(e))
				.ToArray();

			_startExecutors = executors
				.Where(e => e.Method == ExecuteMethod.Start)
				.OrderBy(e => e.Priority)
				.Select(e => new InterfaceReference<IExecuteHolder>(e))
				.ToArray();

			this.SetDirty();
		}

		private void SetIndexes()
		{
			if (_awakeExecutors is { Length: > 0 })
			{
				Set(_awakeExecutors);
			}

			if (_startExecutors is { Length: > 0 })
			{
				Set(_startExecutors);
			}

			return;

			void Set(InterfaceReference<IExecuteHolder>[] executors)
			{
				for (int i = 0; i < executors.Length; i++)
				{
					executors[i].Value.Priority = i;
				}
			}
		}

		private void Awake()
		{
			Handle(_awakeExecutors).Forget();
		}

		private void Start()
		{
			Handle(_startExecutors).Forget();
		}

		private async UniTaskVoid Handle(InterfaceReference<IExecuteHolder>[] executors)
		{
			_source?.Cancel();
			_source = new CancellationTokenSource();

			foreach (var executor in executors.Select(e => e.Value.GetExecutor()))
			{
				if (executor == null)
				{
					continue;
				}
				
				switch (executor.Mode)
				{
					case ExecuteMode.Async:
						await executor.ExecuteAsync();

						break;
					case ExecuteMode.AsyncForget:
						executor.ExecuteAsync().Forget();

						break;
					case ExecuteMode.Sync:
						executor.Execute();

						break;
					default: throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}