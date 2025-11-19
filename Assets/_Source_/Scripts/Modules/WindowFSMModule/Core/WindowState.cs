using System.Threading;
using Cysharp.Threading.Tasks;
using FSMModule;

namespace WindowFSMModule
{
	public abstract class WindowState : State<WindowStateMachine>
	{
		private Window _window;
		private CancellationTokenSource _source;

		public void Init(Window window)
		{
			_window = window;
			_window.Init();
		}

		public override void Enter()
		{
			base.Enter();
			
			_source?.Cancel();
			_source = new CancellationTokenSource();

			TryShow(_source.Token).Forget();
		}

		public override void Exit()
		{
			base.Exit();
			
			_source?.Cancel();
			_source = new CancellationTokenSource();

			TryHide(_source.Token).Forget();
		}

		private async UniTaskVoid TryShow(CancellationToken token)
		{
			await UniTask.Yield();

			if (token.IsCancellationRequested)
			{
				return;
			}
			
			if (_window)
			{
				_window.Show(token);
			}
		}

		private async UniTaskVoid TryHide(CancellationToken token)
		{
			await UniTask.Yield();

			if (token.IsCancellationRequested)
			{
				return;
			}
			
			if (_window)
			{
				_window.Hide(token).Forget();
			}
		}
	}

	public abstract class WindowState<T> : WindowState
	{
	}
}