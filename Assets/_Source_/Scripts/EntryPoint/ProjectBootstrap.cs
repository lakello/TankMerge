namespace miniit.ENTRYPOINT
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
		[Scene]
		[SerializeField]
		private string gameSceneName;
		[SerializeField]
		private ItemsConfig itemsConfig;

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

			DI.Register(actions).DisposeOnQuitGame();
			DI.Register(itemsConfig).DisposeOnQuitGame();

			SceneManager.LoadSceneAsync(gameSceneName);
		}
	}
}