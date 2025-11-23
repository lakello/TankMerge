namespace MiniIT.ENTRYPOINT
{
    using AudioModule;
    using AudioModule.Extensions;
    using AudioModule.Messages;
    using GAME.MERGE.UNIT;
    using GAME.PLAYER;
    using MessageModule;
    using miniit.INPUT;
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

        private void Start()
        {
            new M_PlayLoopClipByType()
            {
                MessageData = new AudioMessageData()
                {
                    ID = AudioID.Background,
                    WaitInit = true,
                }
            }.Publish();
        }

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