using System;
using System.Collections.Generic;

namespace UtilsModule.Extensions
{
	public static class ListExtension
	{
		private static readonly Random Random = new Random();
		
		public static void Shuffle<T>(this IList<T> list)  
		{  
			int n = list.Count;  
			while (n > 1) {  
				n--;  
				int k = Random.Next(n + 1);  
				(list[k], list[n]) = (list[n], list[k]);
			}  
		}
	}
}