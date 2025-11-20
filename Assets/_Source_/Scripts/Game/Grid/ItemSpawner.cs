namespace miniit.GAME.MERGE
{
	using CELL;
	using Doozy.Runtime.UIManager;
	using Doozy.Runtime.UIManager.Components;
	using ITEM;
	using MessageModule;
	using Messages;
	using R3;
	using UnityEngine;

	public class ItemSpawner : MonoBehaviour
	{
		[SerializeField]
		private UIButton button;
		[SerializeField]
		private MergeItem itemPrefab;

		private CompositeDisposable disposable;

		private void Awake()
		{
			MergeItem.InitPool(itemPrefab);
		}

		private void OnEnable()
		{
			button.OnSelectionStateChangedCallback.AddListener(OnSelectionStateChangedCallback);

			disposable = new CompositeDisposable();

			new MergeSuccessMessage().Receive()
				.Subscribe(OnMergeSuccess)
				.AddTo(disposable);
		}

		private void OnDisable()
		{
			button.OnSelectionStateChangedCallback.RemoveListener(OnSelectionStateChangedCallback);
			disposable?.Dispose();
		}

		private void OnMergeSuccess(MergeSuccessMessage mergeSuccessMessage)
		{
			mergeSuccessMessage.TargetCell.SetItem(GetItem(mergeSuccessMessage.Level + 1));
		}

		private void OnSelectionStateChangedCallback(UISelectionState state)
		{
			if (state == UISelectionState.Pressed)
			{
				if (CellsHolder.Instance.CanOccupyCell)
				{
					CellsHolder.Instance.TryOccupyRandomCell(GetItem(1));
				}
			}
		}

		private MergeItem GetItem(int level)
		{
			MergeItem item = MergeItem.Pool.Get();
			item.SetLevel(level);
			item.gameObject.SetActive(true);
			return item;
		}
	}
}