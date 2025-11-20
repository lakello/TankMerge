namespace miniit.GAME.MERGE
{
	using CELL;
	using ITEM;
	using MessageModule;
	using Messages;
	using UnityEngine;

	public class MergeHandler : MonoBehaviour
	{
		private const int HITS_COUNT = 4;

		private readonly RaycastHit[] hitsCache = new RaycastHit[HITS_COUNT];

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

			int hitCount = Physics.BoxCastNonAlloc(
				origin,
				halfExtents,
				Vector3.down,
				hitsCache,
				Quaternion.identity,
				castDistance,
				gridCellLayerMask
			);

			if (hitCount == 0)
			{
				currentCell.ResetItemToPlace();
				return;
			}

			GridCell nearestCell = null;

			for (int i = 0; i < hitCount; i++)
			{
				if (hitsCache[i].collider.TryGetComponent(out GridCell cell))
				{
					if (nearestCell == null
						|| Vector3.Distance(origin, nearestCell.transform.position) > Vector3.Distance(origin, cell.transform.position))
					{
						nearestCell = cell;
					}
				}
			}

			if (nearestCell != null)
			{
				CheckCell();
			}
			else
			{
				currentCell.ResetItemToPlace();
			}

			return;

			void CheckCell()
			{
				if (nearestCell.Item == null || nearestCell.Item == currentCell.Item)
				{
					CellsHolder.Instance.ReleaseCell(currentCell);
					nearestCell.SetItem(item);
				}
				else
				{
					TryMerge();
				}
			}

			void TryMerge()
			{
				if (nearestCell.Item.CurrentLevel == currentCell.Item.CurrentLevel)
				{
					int currentLevel = currentCell.Item.CurrentLevel;

					MergeItem.Pool.Release(nearestCell.Item);
					MergeItem.Pool.Release(currentCell.Item);

					CellsHolder.Instance.ReleaseCell(currentCell);
					CellsHolder.Instance.ReleaseCell(nearestCell);

					new MergeSuccessMessage
					{
						TargetCell = nearestCell,
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