namespace miniit.MERGE
{
	using System;
	using UnityEngine;

	public class GridCell : MonoBehaviour
	{
		[SerializeField]
		private Transform placeTransform;
		
		private MergeItem item;
		
		public MergeItem Item => item;

		public void ResetItemToPlace()
		{
			if (item == null)
			{
				return;
			}
			
			item.transform.position = placeTransform.position;
			item.transform.rotation = placeTransform.rotation;
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