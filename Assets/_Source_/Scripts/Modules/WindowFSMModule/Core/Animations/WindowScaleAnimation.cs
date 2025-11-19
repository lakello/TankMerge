using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UtilsModule.Extensions;

namespace WindowFSMModule.Animations
{
	[Serializable]
	public class WindowScaleAnimation : WindowAnimation
	{
		[SerializeField]
		private GameObject[] _managedGameObjects;
		[SerializeField]
		[Range(0, 1)]
		[ShowIf(nameof(HasHideObjects))]
		private float _manageDelayPercent;
		[SerializeField]
		private RectTransform _rect;
		[SerializeField]
		private float _duration;
		[SerializeField]
		private float _startScale;
		[SerializeField]
		private float _endScale;
		[SerializeField]
		private AnimationCurve _curve;
		
		private float _delay;
		
		private Tween _heightTween;
		private Tween _widthTween;
		
		private bool HasHideObjects => _managedGameObjects?.Length > 0;

		protected override async UniTask InternalPlay(CancellationToken token)
		{
			_rect.localScale = _startScale.ToVector3();

			_heightTween = Tween.ScaleY(_rect, _endScale, _duration, _curve);
			_widthTween = Tween.ScaleX(_rect,_endScale, _duration, _curve);
			
			if (HasHideObjects)
			{
				HandleObjects(token).Forget();
			}
			
			await UniTask.WaitForSeconds(_duration, cancellationToken: token);
		}
		
		private async UniTaskVoid HandleObjects(CancellationToken token)
		{
			_delay = _duration * _manageDelayPercent;

			await UniTask.WaitForSeconds(_delay, cancellationToken: token);

			if (token.IsCancellationRequested)
			{
				return;
			}

			_managedGameObjects.ForEach(g => g.SetActive(false));
		}
		
		protected override void Stop()
		{
			_heightTween.Stop();
			_widthTween.Stop();
		}
	}
}