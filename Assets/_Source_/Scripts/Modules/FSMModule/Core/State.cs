using System;

namespace FSMModule
{
	public abstract class State<TMachine>
		where TMachine : StateMachine<TMachine>
	{
		public event Action<bool> StateChanged;

		public virtual void Enter() =>
			StateChanged?.Invoke(true);

		public virtual void Exit() =>
			StateChanged?.Invoke(false);
	}
}