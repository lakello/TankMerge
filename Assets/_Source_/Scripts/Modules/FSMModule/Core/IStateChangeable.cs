using System;

namespace FSMModule
{
	public interface IStateChangeable<TMachine>
		where TMachine : StateMachine<TMachine>
	{
		public event Action StateChanged;

		public Type CurrentState { get; }

		public void AddListenerTo<TState>(Action<bool> observer)
			where TState : State<TMachine>;

		public void RemoveListenerTo<TState>(Action<bool> observer)
			where TState : State<TMachine>;
	}
}