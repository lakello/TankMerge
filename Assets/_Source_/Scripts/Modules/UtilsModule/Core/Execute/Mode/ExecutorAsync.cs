namespace UtilsModule.Execute
{
	using System;
	using Cysharp.Threading.Tasks;
	using Other;

	public class ExecutorAsync : Executor
	{
		private readonly Func<UniTask> _action;
		private readonly Func<DContainer, UniTask> _containerAction;

		public ExecutorAsync(Func<UniTask> action, bool isForget = false)
		{
			_action = action;

			Mode = isForget
				? ExecuteMode.AsyncForget
				: ExecuteMode.Async;

			NeedContainer = false;
		}
		
		public ExecutorAsync(Func<DContainer, UniTask> action, bool isForget = false)
		{
			_containerAction = action;

			Mode = isForget
				? ExecuteMode.AsyncForget
				: ExecuteMode.Async;
			
			NeedContainer = true;
		}

		public override ExecuteMode Mode { get; }
		public override bool NeedContainer { get; }

		public override async UniTask ExecuteAsync()
		{
			await _action();
		}
		
		public override async UniTask ExecuteAsync(DContainer container)
		{
			await _containerAction(container);
		}
	}
}