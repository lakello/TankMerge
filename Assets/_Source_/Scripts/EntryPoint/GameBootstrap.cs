namespace miniit.ENTRYPOINT
{
	using INPUT;
	using UnityEngine;
	using UtilsModule.Disposable;
	using UtilsModule.Execute;
	using UtilsModule.Execute.Interfaces;
	using UtilsModule.Other;

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

			DI.Resolve<InputActions>().Enable();
		}

		private void OnDisable()
		{
			if (DI.TryResolve(out InputActions input))
			{
				input.Disable();
			}
		}
	}
}