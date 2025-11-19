namespace UtilsModule.Execute
{
	using Cysharp.Threading.Tasks;

	public abstract class Executor
	{
		public abstract ExecuteMode Mode { get; }

		public virtual void Execute()
		{
		}
		
		public virtual UniTask ExecuteAsync()
		{
			return UniTask.CompletedTask;
		}
	}
}