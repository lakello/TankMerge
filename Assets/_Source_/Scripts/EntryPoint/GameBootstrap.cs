namespace miniit.ENTRYPOINT
{
	using UnityEngine;
	using UtilsModule.Disposable;
	using UtilsModule.Execute;
	using UtilsModule.Execute.Interfaces;

	public class GameBootstrap : MonoBehaviour, IExecuteHolder
	{
		public ExecuteMethod Method => ExecuteMethod.Awake;
		public int Priority { get; set; }

		public Executor GetExecutor()
		{
			return new ExecutorSync(Init);
		}

		private void Init()
		{
			SceneDisposableHolder.Create(gameObject.scene.name);
		}
	}
}