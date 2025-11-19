using System;
using System.Collections.Generic;
using System.Reflection;

namespace FSMModule
{
	public abstract partial class StateMachine<TMachine> : IDisposable, IStateMachine<TMachine>
		where TMachine : StateMachine<TMachine>
	{
		private const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

		private readonly Dictionary<Type, State<TMachine>> _states = new Dictionary<Type, State<TMachine>>();

		private State<TMachine> _defaultState;
		private State<TMachine> _previousState;
		private State<TMachine> _currentState;
		private object[] _initParams;

		public event Action StateChanged;

		public Type CurrentState => _currentState.GetType();
		public Type PreviousState => _previousState.GetType();

		public void Dispose()
		{
			_currentState?.Exit();
		}

		public TMachine SetStateInstanceParameters(params object[] initParams)
		{
			_initParams = initParams;

			return (TMachine)this;
		}

		public TMachine ResetStateInstanceParameters()
		{
			_initParams = null;

			return (TMachine)this;
		}

		public void SetDefaultState<TState>()
			where TState : State<TMachine>
		{
			DoWith<TState>(state => _defaultState = state);
		}

		public TMachine AddState<TState>(params object[] initParams)
			where TState : State<TMachine>
		{
			var state = (State<TMachine>)Activator.CreateInstance(
				typeof(TState),
				Flags,
				null,
				initParams.Length > 0
					? initParams
					: _initParams,
				null);

			_states.Add(typeof(TState), state);

			return (TMachine)this;
		}

		public void EnterIn<TState>()
			where TState : State<TMachine>
		{
			if (typeof(TState) == _currentState?.GetType())
			{
				return;
			}

			_previousState = _currentState;

			DoWith<TState>(ChangeState);
		}
		
		public void EnterInDefaultState<TState>()
			where TState : State<TMachine>
		{
			if (_defaultState != null)
			{
				ChangeState(_defaultState);
			}
			else
			{
				throw new NullReferenceException("It is impossible to revert to the default state.");
			}
		}

		public void EnterInPreviousState()
		{
			if (_previousState != null)
			{
				ChangeState(_previousState);
			}
			else if (_defaultState != null)
			{
				ChangeState(_defaultState);
			}
			else
			{
				throw new NullReferenceException("It is impossible to revert to the previous state.");
			}
		}

		public void AddListenerTo<TState>(Action<bool> observer)
			where TState : State<TMachine>
		{
			DoWith<TState>((state) => state.StateChanged += observer);
		}

		public void RemoveListenerTo<TState>(Action<bool> observer)
			where TState : State<TMachine>
		{
			DoWith<TState>((state) => state.StateChanged -= observer);
		}

		protected State<TMachine> TryGetState(Type stateType)
		{
			return _states.GetValueOrDefault(stateType);
		}

		private void ChangeState(State<TMachine> state)
		{
			_currentState?.Exit();
			_currentState = state;
			_currentState.Enter();

			StateChanged?.Invoke();
		}

		private void DoWith<TState>(Action<State<TMachine>> action)
			where TState : State<TMachine>
		{
			if (_states.TryGetValue(typeof(TState), out var state))
			{
				action(state);
			}
			else
			{
				throw new ArgumentException($"{typeof(TState).Name} is not registered");
			}
		}
	}
}