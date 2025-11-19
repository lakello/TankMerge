using UnityEngine;

namespace UtilsModule.Extensions
{
	public static class FloatExtensions
	{
		public static float Normalize(this float value, float min, float max)
		{
			return (value - min) / (max - min);
		}
		
		public static float Invert(this float value)
		{
			return 1 - value;
		}
		
		public static Vector3 ToVector3(this float value)
		{
			return new Vector3(value, value, value);
		}
		
		public static Vector3 ToVector3Y(this float value)
		{
			return new Vector3(0, value, 0);
		}
	}
}