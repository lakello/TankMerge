using UnityEngine;
using UtilsModule.Extensions;
using UtilsModule.Other;

namespace UtilsModule.Animations
{
	public class SinScaleAnimation : MonoBehaviour
	{
		[SerializeField] private SinData _sinData;

		private void Update()
		{
			float value = _sinData.Get(1);
			transform.localScale = value.ToVector3();
		}
	}
}