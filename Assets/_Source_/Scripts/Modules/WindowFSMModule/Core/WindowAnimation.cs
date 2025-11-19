using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting;
using UtilsModule.Other;
using WindowFSMModule.Utils;

namespace WindowFSMModule
{
	[Serializable]
	[Preserve]
	public abstract class WindowAnimation
	{
		[SerializeField]
		private bool _allStates = true;
		[SerializeField]
		[HideIf("_allStates")]
		private ValidWindowState[] _includeWindowStates;
		[SerializeField]
		[HideIf("_allStates")]
		private ValidWindowState[] _excludeWindowStates;

		[field: SerializeField] public ExecuteMode Mode { get; private set; }

		public async UniTask Play(CancellationToken token)
		{
			if (_allStates || CheckValidState())
			{
				Stop();
				
				await InternalPlay(token);
			}

			Stop();
		}

		protected abstract UniTask InternalPlay(CancellationToken token);
		protected abstract void Stop();

		private bool CheckValidState()
		{
			var machine = DI.Resolve<WindowStateMachine>();

			bool isValid = true;

			if (_includeWindowStates is { Length: > 0 })
			{
				isValid = _includeWindowStates.IsValidArray(machine);
			}

			if (_excludeWindowStates is { Length: > 0 })
			{
				isValid = _excludeWindowStates.IsValidArray(machine) == false;
			}

			return isValid;
		}

		public enum ExecuteMode
		{
			Wait,
			Forget,
		}
	}
}