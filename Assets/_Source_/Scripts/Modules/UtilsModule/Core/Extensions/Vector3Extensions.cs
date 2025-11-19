using UnityEngine;

namespace TowerOfHell.Tools
{
	public static class Vector3Extensions
	{
		public static void SetAll(this Vector3 vector, ref Vector3 value)
		{
			vector.Set(value.x, value.y, value.z);
		}
		
		public static void SetAll(this Vector3 vector, Vector3 value)
		{
			vector.Set(value.x, value.y, value.z);
		}
	}
}