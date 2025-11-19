using Cysharp.Threading.Tasks;
using FSMModule;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UtilsModule.Other;

namespace WindowFSMModule.Utils
{
	public class StateView : SerializedMonoBehaviour
	{
		[OdinSerialize] private ValidWindowState[] _validStates;

		private IStateChangeable<WindowStateMachine> _windowStateMachine;

		private void Awake()
		{
			_windowStateMachine = DI.Resolve<WindowStateMachine>();
			_windowStateMachine.StateChanged += OnStateChanged;
		}

		private void OnDestroy()
		{
			_windowStateMachine.StateChanged -= OnStateChanged;
		}

		private async void OnStateChanged()
		{
			await UniTask.Yield();

			if (_validStates.IsValidArray(_windowStateMachine))
			{
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
}