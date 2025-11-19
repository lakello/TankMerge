#if UNITY_EDITOR
using FSMModule.Debug;

namespace FSMModule
{
	public partial class StateMachine<TMachine> : IDebugStateMachine
		where TMachine : StateMachine<TMachine>
	{
		protected StateMachine()
		{
			StateMachineInstancesContainer.Machines.Add(this);
		}

		public string MachineName => GetType().Name;
		public string StateName => _currentState == null ? string.Empty : CurrentState.Name;
	}
}
#endif