namespace miniit.GAME.MERGE
{
	using CELL;
	using ITEM;
	using MessageModule;
	using Messages;
	using UnityEngine;

	public class MergeHandler : MonoBehaviour
	{
		[SerializeField]
		private DragHandler dragHandler;
		[SerializeField]
		private LayerMask gridCellLayerMask = 0;
		[SerializeField]
		private float castDistance = 2.0f;

		private void OnEnable()
		{
			dragHandler.DragEnded += OnDragEnded;
		}

		private void OnDisable()
		{
			dragHandler.DragEnded -= OnDragEnded;
		}

		private void OnDragEnded(GridCell currentCell)
		{
			if (currentCell == null || currentCell.Item == null)
			{
				return;
			}

			MergeItem item = currentCell.Item;

			Transform itemTransform = item.transform;

			BoxCollider boxCollider = null;
			bool hasCollider = item.TryGetComponent(out boxCollider);
			if (!hasCollider || boxCollider == null)
			{
				currentCell.ResetItemToPlace();
				return;
			}

			Vector3 halfExtents = boxCollider.bounds.extents;

			Vector3 origin = itemTransform.position;
			origin.y += halfExtents.y;

			RaycastHit hitInfo;
			bool hit = Physics.BoxCast(
				origin,
				halfExtents,
				Vector3.down,
				out hitInfo,
				Quaternion.identity,
				castDistance,
				gridCellLayerMask
			);

			if (!hit)
			{
				currentCell.ResetItemToPlace();
				return;
			}

			GridCell gridCell = hitInfo.collider.GetComponent<GridCell>();
			if (gridCell == null)
			{
				currentCell.ResetItemToPlace();
				return;
			}

			if (gridCell.Item == null)
			{
				CellsHolder.Instance.ReleaseCell(currentCell);
				gridCell.SetItem(item);
			}
			else
			{
				TryMerge();
			}

			return;

			void TryMerge()
			{
				if (gridCell.Item.CurrentLevel == currentCell.Item.CurrentLevel)
				{
					int currentLevel = currentCell.Item.CurrentLevel;

					MergeItem.Pool.Release(gridCell.Item);
					MergeItem.Pool.Release(currentCell.Item);

					CellsHolder.Instance.ReleaseCell(currentCell);
					CellsHolder.Instance.ReleaseCell(gridCell);

					new MergeSuccessMessage
					{
						TargetCell = gridCell,
						Level = currentLevel,
					}.Publish();
				}
				else
				{
					currentCell.ResetItemToPlace();
				}
			}
		}
	}
}