namespace miniit.GAME.MERGE.ITEM
{
	using System;
	using UnityEngine;
	using UnityEngine.Pool;

	public class MergeItem : MonoBehaviour
	{
		private const int START_LEVEL = 1;

		public static ObjectPool<MergeItem> Pool { get; private set; }

		public int CurrentLevel { get; private set; }

		private void OnDisable()
		{
			CurrentLevel = 0;
		}

		public static void InitPool(MergeItem prefab)
		{
			GameObject parent = new GameObject($"{nameof(MergeItem)}:POOL");

			prefab.gameObject.SetActive(false);

			Pool = new ObjectPool<MergeItem>(
				createFunc: () => Instantiate(prefab, parent.transform),
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