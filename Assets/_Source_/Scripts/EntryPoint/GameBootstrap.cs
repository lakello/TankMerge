namespace MiniIT.ENTRYPOINT
{
    using GAME.MERGE.UNIT;
    using INPUT;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UtilsModule.Disposable;
    using UtilsModule.Execute;
    using UtilsModule.Execute.Interfaces;
    using UtilsModule.Other;

    public class GameBootstrap : SerializedMonoBehaviour, IExecuteHolder
    {
        [SerializeField] private MergeUnit unitPrefab;

        public ExecuteMethod Method => ExecuteMethod.Awake;
        public int Priority { get; set; }

        public Executor GetExecutor()
        {
            return new ExecutorSync(Init);
        }

        private void Init()
        {
            SceneDisposableHolder.Create(gameObject.scene.name);
            GlobalData.Container.Resolve<InputActions>().Enable();

            MergeUnit.InitPool(unitPrefab);
        }

        private void OnDisable()
        {
            if (GlobalData.Container.TryResolve(out InputActions input))
            {
                input.Disable();
            }
        }
    }
}