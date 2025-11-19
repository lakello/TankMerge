namespace miniit.MERGE
{
	using Doozy.Runtime.UIManager;
	using Doozy.Runtime.UIManager.Components;
	using UnityEngine;

	public class ItemSpawner : MonoBehaviour
	{
		[SerializeField]
		private UIButton button;
		[SerializeField]
		private MergeItem itemPrefab;

		private void OnEnable()
		{
			button.OnSelectionStateChangedCallback.AddListener(OnSelectionStateChangedCallback);
		}

		private void OnDisable()
		{
			button.OnSelectionStateChangedCallback.RemoveListener(OnSelectionStateChangedCallback);
		}

		private void OnSelectionStateChangedCallback(UISelectionState state)
		{
			if (state == UISelectionState.Pressed)
			{
				if (CellsHolder.Instance.CanOccupyCell)
				{
					CellsHolder.Instance.TryOccupyCell(Instantiate(itemPrefab));
				}
			}
		}
	}
}