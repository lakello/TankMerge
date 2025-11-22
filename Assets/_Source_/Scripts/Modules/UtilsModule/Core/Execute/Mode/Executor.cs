namespace UtilsModule.Execute
{
	using Cysharp.Threading.Tasks;
	using Other;

	public abstract class Executor
	{
		public abstract ExecuteMode Mode { get; }
		public abstract bool NeedContainer { get; }

		public virtual void Execute()
		{
		}
		
		public virtual UniTask ExecuteAsync()
		{
			return UniTask.CompletedTask;
		}
		
		public virtual void Execute(DContainer container)
		{
		}
		
		public virtual UniTask ExecuteAsync(DContainer container)
		{
			return UniTask.CompletedTask;
		}
	}
}