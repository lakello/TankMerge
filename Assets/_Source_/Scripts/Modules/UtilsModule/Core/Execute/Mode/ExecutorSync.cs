namespace UtilsModule.Execute
{
	using System;

	public class ExecutorSync : Executor
	{
		private readonly Action _action;

		public ExecutorSync(Action action)
		{
			_action = action;
		}

		public override ExecuteMode Mode => ExecuteMode.Sync;

		public override void Execute()
		{
			_action();
		}
	}
}