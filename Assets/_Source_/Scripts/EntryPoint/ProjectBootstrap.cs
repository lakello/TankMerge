namespace miniit.ENTRYPOINT
{
	using NaughtyAttributes.Core.DrawerAttributes;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UtilsModule.Disposable;
	using UtilsModule.Execute;
	using UtilsModule.Execute.Interfaces;

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
			SceneManager.LoadSceneAsync(gameSceneName);
		}
	}
}