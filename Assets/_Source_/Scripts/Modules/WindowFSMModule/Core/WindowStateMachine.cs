using FSMModule;

namespace WindowFSMModule
{
	public class WindowStateMachine : StateMachine<WindowStateMachine>
	{
		public TState TryGetStateByWindow<TState>(Window window)
			where TState : State<WindowStateMachine> =>
			(TState)TryGetState(window.WindowType);
	}
}