using UnityEngine;
using UnityEngine.UI;

namespace UtilsModule.Other
{
	[RequireComponent(typeof(Button))]
	public abstract class ButtonHandler : MonoBehaviour
	{
		private Button _button;

		private Button Button
		{
			get
			{
				if (_button == false)
				{
					_button = GetComponent<Button>();
				}

				if (_button == false)
				{
					_button = gameObject.AddComponent<Button>();
				}

				return _button;
			}
		}

		public void SetInteractable(bool interactable)
		{
			Button.interactable = interactable;
		}

		private void OnEnable()
		{
			Button.onClick.AddListener(OnClick);
			InternalEnable();
		}

		private void OnDisable()
		{
			Button.onClick.RemoveListener(OnClick);
			InternalDisable();
		}

		private void OnDestroy()
		{
			InternalDestroy();
		}

		protected virtual void InternalEnable()
		{
		}
		
		protected virtual void InternalDisable()
		{
		}
		
		protected virtual void InternalDestroy()
		{
		}

		protected abstract void OnClick();
	}
}