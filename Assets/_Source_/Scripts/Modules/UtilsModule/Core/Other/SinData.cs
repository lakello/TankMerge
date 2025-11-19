using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UtilsModule.Other
{
	[Serializable]
	[HideLabel]
	public struct SinData
	{
		[SerializeField]
		private bool _useRandom;
		[SerializeField]
		[HideIf("_useRandom")]
		private float _amplitude;
		[SerializeField]
		[HideIf("_useRandom")]
		private float _frequency;
		[SerializeField]
		[ShowIf("_useRandom")]
		private Vector2 _amplitudeRange;
		[SerializeField]
		[ShowIf("_useRandom")]
		private Vector2 _frequencyRange;

		private float _randomAmplitude;
		private float _randomFrequency;
		private bool _isInitialized;

		public float Get(float baseValue = 0)
		{
			float amplitude;
			float frequency;

			if (_useRandom && _isInitialized == false)
			{
				_isInitialized = true;
				_randomAmplitude = UnityEngine.Random.Range(_amplitudeRange.x, _amplitudeRange.y);
				_randomFrequency = UnityEngine.Random.Range(_frequencyRange.x, _frequencyRange.y);
			}

			if (_useRandom)
			{
				amplitude = _randomAmplitude;
				frequency = _randomFrequency;
			}
			else
			{
				amplitude = _amplitude;
				frequency = _frequency;
			}

			return baseValue + amplitude * Mathf.Sin(Time.time * frequency);
		}
	}
}