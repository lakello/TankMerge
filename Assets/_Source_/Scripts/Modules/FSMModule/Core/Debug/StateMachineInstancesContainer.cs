#if UNITY_EDITOR
using System.Collections.Generic;

namespace FSMModule.Debug
{
	public static class StateMachineInstancesContainer
	{
		public static readonly List<IDebugStateMachine> Machines = new List<IDebugStateMachine>();
	}
}
#endif