using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace WindowFSMModule.Animations
{
	[Serializable]
	public class WindowAlphaAnimation : WindowAnimation
	{
		[SerializeField]
		private Image _image;
		[SerializeField]
		private float _startAlpha;
		[SerializeField]
		private float _endAlpha;
		[SerializeField]
		private float _duration;

		private Tween _tweener;

		protected override async UniTask InternalPlay(CancellationToken token)
		{
			var color = _image.color;
			color.a = _startAlpha;
			_image.color = color;

			color.a = _endAlpha;

			_tweener = Tween.Color(_image, color, _duration);

			await UniTask.WhenAll(_tweener.ToYieldInstruction().ToUniTask(cancellationToken: token));
		}

		protected override void Stop()
		{
			_tweener.Stop();
		}
	}
}