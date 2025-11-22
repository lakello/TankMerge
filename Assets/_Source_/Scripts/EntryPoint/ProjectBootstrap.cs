namespace MiniIT.ENTRYPOINT
{
    using GAME.Settings;
    using INPUT;
    using NaughtyAttributes.Core.DrawerAttributes;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UtilsModule.Disposable;
    using UtilsModule.Execute;
    using UtilsModule.Execute.Interfaces;
    using UtilsModule.Other;

    public class ProjectBootstrap : MonoBehaviour, IExecuteHolder
    {
        [Scene] [SerializeField] private string     gameSceneName;
        [SerializeField]         private TankConfig tankConfig;

        public ExecuteMethod Method => ExecuteMethod.Awake;
        public int Priority { get; set; }

        public Executor GetExecutor()
        {
            return new ExecutorSync(Init);
        }

        private void Init()
        {
            GlobalDisposableHolder.Create();

            InputActions actions = new InputActions();

            GlobalData.Container.Register(actions).DisposeOnQuitGame();
            GlobalData.Container.Register(tankConfig).DisposeOnQuitGame();

            SceneManager.LoadSceneAsync(gameSceneName);
        }
    }
}