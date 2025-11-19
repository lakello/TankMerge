using System;
using R3;

namespace FSMModule.Transit
{
	public class TransitionInitializer<TMachine> : IDisposable
		where TMachine : StateMachine<TMachine>
	{
		private readonly TMachine _stateMachine;
		private readonly CompositeDisposable _disposable = new CompositeDisposable();

		public TransitionInitializer(TMachine stateMachine) =>
			_stateMachine = stateMachine;

		public void Dispose()
		{
			_disposable.Dispose();
		}

		public TransitionInitializer<TMachine> InitTransition<TTargetState, T>(
			Observable<T> observable,
			Action observer = null)
			where TTargetState : State<TMachine>
		{
			var transition = new Transition<TMachine, TTargetState>(_stateMachine);

			var disposable = new CompositeDisposable();

			observable.Subscribe(_ =>
				{
					transition.Transit();
					observer?.Invoke();
				})
				.AddTo(disposable);

			_disposable.Add(disposable);

			return this;
		}
	}
}