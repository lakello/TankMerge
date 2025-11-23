using AudioModule.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace AudioModule
{
	[RequireComponent(typeof(Button))]
	public class AudioButton : MonoBehaviour
	{
		private Button _button;

		private Button Button => _button ??= GetComponent<Button>();

		private void OnEnable()
		{
			Button.onClick.AddListener(OnClick);
		}

		private void OnDisable()
		{
			Button.onClick.RemoveListener(OnClick);
		}

		private void OnClick()
		{
			AudioID.UI.PlayOneShot();
		}
	}
}