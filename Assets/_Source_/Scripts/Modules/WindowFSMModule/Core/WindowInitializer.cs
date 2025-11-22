using System.Collections.Generic;
using UnityEngine;
using UtilsModule.Execute;
using UtilsModule.Execute.Interfaces;
using UtilsModule.Other;

namespace WindowFSMModule
{
	public sealed class WindowInitializer : MonoBehaviour, IExecuteHolder
	{
		[SerializeField] private List<Window> _windows;

		public ExecuteMethod Method => ExecuteMethod.Awake;

		public int Priority { get; set; }

		public Executor GetExecutor()
		{
			return new ExecutorSync(WindowsInit);
		}

		private void WindowsInit()
		{
			var machine = GlobalData.Container.Resolve<WindowStateMachine>();
			
			foreach (var window in _windows)
			{
				var state = machine.TryGetStateByWindow<WindowState>(window);

				state.Init(window);
				state.Exit();

				if (state.GetType() == machine.CurrentState)
				{
					state.Enter();
				}
			}
		}
	}
}