using UnityEngine;
using UtilsModule.Other;

namespace UtilsModule.Animations
{
	public class SinMoveAnimation : MonoBehaviour
	{
		[SerializeField] private SinData _sinData;

		private float _startHeight;

		private void Awake()
		{
			_startHeight = transform.position.y;
		}

		private void Update()
		{
			float value = _sinData.Get(_startHeight);
			var position = transform.position;
			position.y = value;
			transform.position = position;
		}
	}
}