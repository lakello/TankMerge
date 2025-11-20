namespace miniit.GAME.MERGE.CELL
{
	using System;
	using ITEM;
	using UnityEngine;

	public class GridCell : MonoBehaviour
	{
		[SerializeField]
		private Transform placePoint;

		private MergeItem item;

		public MergeItem Item => item;

		public void ResetItemToPlace()
		{
			if (item == null)
			{
				return;
			}

			item.transform.position = placePoint.position;
			item.transform.rotation = placePoint.rotation;
		}

		public void SetItem(MergeItem item)
		{
			if (this.item == null)
			{
				this.item = item;
				ResetItemToPlace();
				return;
			}

			throw new Exception("Cannot set more than once");
		}

		public void Release()
		{
			item = null;
		}
	}
}