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
    using Other;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using ZLinq;

    public class OrderExecutor : MonoBehaviour
    {
        [SerializeField]
        private Context _context;
        [SerializeField]
        private MonoContainer _monoContainer;
        [SerializeField]
        [ShowIf("IsScene")]
        private InterfaceReference<IExecuteHolder>[] _awakeExecutors;
        [SerializeField]
        [ShowIf("IsScene")]
        private InterfaceReference<IExecuteHolder>[] _startExecutors;
        [SerializeField]
        [ShowIf("IsLocal")]
        private GameObjectExecutorsHolder[] _executorHolders;

        private CancellationTokenSource _source;

        private bool IsScene => _context == Context.Scene;
        private bool IsLocal => _context == Context.Local;

        private enum Context
        {
            Scene,
            Local,
        }

        [Button]
        [ShowIf("IsScene")]
        private void FindExecutors()
        {
            SetIndexes();

            GameObject[] objects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);

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

            return;

            void SetIndexes()
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
        }

        private void Awake()
        {
            switch (_context)
            {
                case Context.Scene:
                    Handle(_awakeExecutors
                            .AsValueEnumerable()
                            .Select(e => e.Value.GetExecutor())
                            .ToArray())
                        .Forget();
                    break;
                case Context.Local:
                    Handle(GetLocalExecutors(ExecuteMethod.Awake)).Forget();
                    break;

                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void Start()
        {
            switch (_context)
            {
                case Context.Scene:
                    Handle(_startExecutors
                            .AsValueEnumerable()
                            .Select(e => e.Value.GetExecutor())
                            .ToArray())
                        .Forget();
                    break;
                case Context.Local:
                    Handle(GetLocalExecutors(ExecuteMethod.Start)).Forget();
                    break;

                default: throw new ArgumentOutOfRangeException();
            }
        }

        private Executor[] GetLocalExecutors(ExecuteMethod method)
        {
            return _executorHolders.AsValueEnumerable()
                .SelectMany(e => e.Holders)
                .Where(e => e.Method == method)
                .Select(e => e.GetExecutor())
                .ToArray();
        }

        private async UniTaskVoid Handle(Executor[] executors)
        {
            _source?.Cancel();
            _source = new CancellationTokenSource();

            foreach (var executor in executors)
            {
                if (executor == null)
                {
                    continue;
                }

                switch (executor.Mode)
                {
                    case ExecuteMode.Async:
                        if (executor.NeedContainer)
                        {
                            await executor.ExecuteAsync(_monoContainer.Container);
                        }
                        else
                        {
                            await executor.ExecuteAsync();
                        }

                        break;
                    case ExecuteMode.AsyncForget:
                        if (executor.NeedContainer)
                        {
                            executor.ExecuteAsync(_monoContainer.Container).Forget();
                        }
                        else
                        {
                            executor.ExecuteAsync().Forget();
                        }

                        break;
                    case ExecuteMode.Sync:
                        if (executor.NeedContainer)
                        {
                            executor.Execute(_monoContainer.Container);
                        }
                        else
                        {
                            executor.Execute();
                        }

                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}