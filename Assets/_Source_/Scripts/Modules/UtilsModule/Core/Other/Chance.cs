using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UtilsModule.Other
{
	public class Chance
	{
		public static bool Default(float chance)
		{
			if (chance == 0)
			{
				return false;
			}

			if (chance == 100)
			{
				return true;
			}
			
			return Random.Range(0, 100) <= chance;
		}

		public static T FromWeight<T>(WeightChanceData<T>[] data, float totalWeight = -1, bool useShuffle = false)
		{
			if (data == null || data.Length == 0)
			{
				return default;
			}
			
			if (useShuffle)
			{
				data = Shuffle(data).ToArray();
			}

			if (totalWeight == -1)
			{
				totalWeight = data.Sum(d => d.Weight);
			}

			float random = Random.Range(0f, totalWeight);

			for (int i = 0; i < data.Length; i++)
			{
				if (random < data[i].Weight)
				{
					return data[i].Data;
				}

				random -= data[i].Weight;
			}

			int index = Random.Range(0, data.Length);
			index = Mathf.Clamp(index, 0, data.Length - 1);
			
			return data[index].Data;
		}
		
		private static IList<T> Shuffle<T>(IList<T> list)  
		{  
			var listShuffled = new List<T>(list);
			
			int n = list.Count;  
			
			while (n-- > 1)
			{
				int k = Random.Range(0, listShuffled.Count);
				(listShuffled[k], listShuffled[n]) = (listShuffled[n], listShuffled[k]);
			}  
			
			return listShuffled;
		}
		
		public struct WeightChanceData<T>
		{
			public T Data;
			public float Weight;
		}
	}
}