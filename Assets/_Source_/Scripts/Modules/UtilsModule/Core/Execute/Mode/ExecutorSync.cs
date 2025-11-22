namespace UtilsModule.Execute
{
	using System;
	using Other;

	public class ExecutorSync : Executor
	{
		private readonly Action _action;
		private readonly Action<DContainer> _containerAction;

		public ExecutorSync(Action action)
		{
			_action = action;
			NeedContainer = false;
		}

		public ExecutorSync(Action<DContainer> containerAction)
		{
			_containerAction = containerAction;
			NeedContainer = true;
		}

		public override ExecuteMode Mode => ExecuteMode.Sync;
		public override bool NeedContainer { get; }

		public override void Execute()
		{
			_action();
		}
		
		public override void Execute(DContainer container)
		{
			_containerAction(container);
		}
	}
}