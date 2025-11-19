namespace UtilsModule.Execute
{
	using System;
	using Cysharp.Threading.Tasks;

	public class ExecutorAsync : Executor
	{
		private readonly Func<UniTask> _action;

		public ExecutorAsync(Func<UniTask> action, bool isForget = false)
		{
			_action = action;

			Mode = isForget
				? ExecuteMode.AsyncForget
				: ExecuteMode.Async;
		}

		public override ExecuteMode Mode { get; }

		public override async UniTask ExecuteAsync()
		{
			await _action();
		}
	}
}