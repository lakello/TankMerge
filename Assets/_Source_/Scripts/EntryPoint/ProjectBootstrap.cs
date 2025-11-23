namespace MiniIT.ENTRYPOINT
{
    using AudioModule;
    using GAME.PLAYER;
    using GAME.Settings;
    using miniit.INPUT;
    using NaughtyAttributes.Core.DrawerAttributes;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.SceneManagement;
    using UtilsModule.Disposable;
    using UtilsModule.Execute;
    using UtilsModule.Execute.Interfaces;
    using UtilsModule.Other;

    public class ProjectBootstrap : MonoBehaviour, IExecuteHolder
    {
        [Scene] [SerializeField] private string         gameSceneName;
        [SerializeField]         private TankDataHolder tankDataHolder;
        [SerializeField]         private AssetReference audioDataRef;
        [SerializeField]         private AudioSource    audioSource;
        [SerializeField]         private AudioSource    pointAudioSource;

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
            GlobalData.Container.Register(new Wallet()).DisposeOnQuitGame();
            GlobalData.Container.Register(tankDataHolder).DisposeOnQuitGame();
            
            _ = new AudioService(audioDataRef, pointAudioSource, audioSource);

            SceneManager.LoadSceneAsync(gameSceneName);
        }
    }
}