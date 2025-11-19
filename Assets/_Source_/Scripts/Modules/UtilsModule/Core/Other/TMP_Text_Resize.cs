using TMPro;
using UnityEngine;

namespace UtilsModule.Other
{
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(TMP_Text))]
	public class TMP_Text_Resize : MonoBehaviour
	{
		private RectTransform _rectTransform;
		private TMP_Text _output;
		private float _defaultWidth;
		private bool _initialized;

		private void Awake()
		{
			if (_initialized == false)
			{
				Init();
			}
		}

		private void Init()
		{
			_rectTransform = GetComponent<RectTransform>();
			_output = GetComponent<TMP_Text>();
			_defaultWidth = _rectTransform.sizeDelta.x;
			_initialized = true;
		}

		public void SetText(string text)
		{
			if (_initialized == false)
			{
				Init();
			}

			_output.text = text;

			var size = _rectTransform.sizeDelta;
			size.x = _defaultWidth * _output.text.Length;

			_rectTransform.sizeDelta = size;
		}
	}
}