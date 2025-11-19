namespace miniit.ENTRYPOINT
{
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

			SceneManager.LoadSceneAsync(gameSceneName);
		}
	}
}