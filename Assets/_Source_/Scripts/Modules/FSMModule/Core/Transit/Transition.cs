namespace FSMModule.Transit
{
	public class Transition<TMachine, TTargetState>
		where TMachine : StateMachine<TMachine>
		where TTargetState : State<TMachine>
	{
		private readonly StateMachine<TMachine> _machine;

		public Transition(StateMachine<TMachine> stateMachine)
		{
			_machine = stateMachine;
		}

		public void Transit()
		{
			if (_machine.CurrentState?.GetType() != typeof(TTargetState))
			{
				_machine.EnterIn<TTargetState>();
			}
		}
	}
}