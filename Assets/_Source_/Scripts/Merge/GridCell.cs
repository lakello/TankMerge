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
		
		public void SetItem(MergeItem item)
		{
			if (this.item == null)
			{
				this.item = item;
				this.item.transform.position = placeTransform.position;
				this.item.transform.rotation = placeTransform.rotation;
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