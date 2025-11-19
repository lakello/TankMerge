#if UNITY_EDITOR
namespace FSMModule.Debug
{
	public interface IDebugStateMachine
	{
		public string MachineName { get; }
		public string StateName { get; }
	}
}
#endif