using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Scripting;
using UtilsModule.Other;

namespace WindowFSMModule
{
	[Preserve]
	public abstract class Window : SerializedMonoBehaviour
	{
		[SerializeField] private WindowAnimation[] _showAnimations;
		[SerializeField] private WindowAnimation[] _hideAnimations;
		[OdinSerialize] private IWindowShowHandler[] _showHandlers;
		[OdinSerialize] private IWindowHideHandler[] _hideHandlers;

		public abstract Type WindowType { get; }

		public virtual void Init()
		{
			gameObject.SetActive(false);
		}

		public void Show(CancellationToken token)
		{
			gameObject.SetActive(true);

			if (_showAnimations != null)
			{
				HandleWindow(_showAnimations, token).Forget();
			}

			_showHandlers?.ForEach(h => h.OnShow());
		}

		public async UniTaskVoid Hide(CancellationToken token)
		{
			if (gameObject.activeSelf == false && DI.Resolve<WindowStateMachine>().CurrentState != WindowType)
			{
				return;
			}
			
			if (_hideAnimations != null)
			{
				await HandleWindow(_hideAnimations, token);
			}
			
			gameObject.SetActive(false);

			_hideHandlers?.ForEach(h => h.OnHide());
		}

		private async UniTask HandleWindow(WindowAnimation[] animations, CancellationToken token)
		{
			if (animations is { Length: > 0 })
			{
				foreach (var windowAnimation in animations)
				{
					if (windowAnimation == null)
					{
						continue;
					}
					
					if (token.IsCancellationRequested == false)
					{
						switch (windowAnimation.Mode)
						{
							case WindowAnimation.ExecuteMode.Wait:
								await windowAnimation.Play(token);
								break;
							case WindowAnimation.ExecuteMode.Forget:
								windowAnimation.Play(token).Forget();
								break;

							default:
								throw new ArgumentOutOfRangeException();
						}
					}
					else
					{
						break;
					}
				}
			}
		}
	}
}