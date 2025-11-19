using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UtilsModule.TransitionSystem
{
	[RequireComponent(typeof(RectTransform))]
	public class SizeDeltaAnimator : MonoBehaviour
	{
		[SerializeField]
		private Vector2 _deltaRangeX;
		[SerializeField]
		private Vector2 _deltaRangeY;
		[SerializeField]
		private float _duration;
		[SerializeField]
		private AnimationCurve _curve;
		[SerializeField]
		private bool _isActivateImageInEnd;
		[SerializeField]
		[ShowIf(nameof(_isActivateImageInEnd))]
		private Image _image;

		private CancellationTokenSource _source;
		private RectTransform _rectTransform;

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();

			if (_isActivateImageInEnd)
				_image.gameObject.SetActive(false);
		}

		private void OnDisable()
		{
			_source?.Cancel();
		}

		public async UniTask Animate()
		{
			if (_source is { IsCancellationRequested: false })
			{
				_source.Cancel();
			}

			_source = new CancellationTokenSource();
			
			var token = _source.Token;

			var time = 0f;

			if (_isActivateImageInEnd)
				_image.gameObject.SetActive(false);

			_rectTransform.sizeDelta = new Vector2(_deltaRangeX.x, _deltaRangeY.x);

			while (time < _duration && token.IsCancellationRequested == false)
			{
				var value = _curve.Evaluate(time / _duration);
				var deltaX = Mathf.Lerp(_deltaRangeX.x, _deltaRangeX.y, value);
				var deltaY = Mathf.Lerp(_deltaRangeY.x, _deltaRangeY.y, value);

				_rectTransform.sizeDelta = new Vector2(deltaX, deltaY);

				time += Time.deltaTime;

				await UniTask.Yield();
			}

			if (token.IsCancellationRequested ==  false)
				_rectTransform.sizeDelta = new Vector2(_deltaRangeX.y, _deltaRangeY.y);

			if (token.IsCancellationRequested)
			{
				return;
			}
			
			if (_isActivateImageInEnd)
				_image.gameObject.SetActive(true);
		}
	}
}