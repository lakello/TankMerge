namespace MiniIT.ENTRYPOINT
{
    using GAME.MERGE.UNIT;
    using GAME.PLAYER;
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
            GlobalData.Container.Resolve<InputActions>().Enable();

            MergeUnit.InitPool(unitPrefab);

            new RewardHandler().DisposeOnExitScene(gameObject.scene.name);
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