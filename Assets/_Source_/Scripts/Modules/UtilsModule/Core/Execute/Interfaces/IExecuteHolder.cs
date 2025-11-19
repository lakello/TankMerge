namespace UtilsModule.Execute.Interfaces
{
	public interface IExecuteHolder
	{
		public ExecuteMethod Method { get; }
		public int Priority { get; set; }
		
		public Executor GetExecutor();
	}
}