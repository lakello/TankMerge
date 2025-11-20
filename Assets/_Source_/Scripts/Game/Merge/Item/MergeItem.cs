namespace miniit.GAME.MERGE.ITEM
{
	using System;
	using UnityEngine;
	using UnityEngine.Pool;

	public class MergeItem : MonoBehaviour
	{
		private const int START_LEVEL = 1;

		[SerializeField]
		private Transform grabPoint;
		
		public static ObjectPool<MergeItem> Pool { get; private set; }

		public int CurrentLevel { get; private set; }
		public Transform GrabPoint => grabPoint;

		public static void InitPool(MergeItem prefab)
		{
			GameObject parent = new GameObject($"{nameof(MergeItem)}:POOL");

			Pool = new ObjectPool<MergeItem>(
				createFunc: () =>
				{
					MergeItem item = Instantiate(prefab, parent.transform);
					item.gameObject.SetActive(false);
					return item;
				},
				actionOnRelease: item => item.gameObject.SetActive(false));
		}

		public void SetLevel(int level)
		{
			if (level < START_LEVEL)
			{
				throw new ArgumentException($"Level must be less than START_LEVEL:({START_LEVEL})");
			}

			CurrentLevel = level;
		}
	}
}