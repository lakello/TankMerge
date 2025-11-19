namespace UtilsModule.Extensions
{
	using UnityEngine;

	public static class LogExtensions
	{
		public static void Log(this string message)
		{
			Debug.Log(message);
		}

		public static void LogEditor(this string message)
		{
#if UNITY_EDITOR
			Debug.Log(message);
#endif
		}
		
		public static void LogWarning(this string message)
		{
			Debug.LogWarning(message);
		}

		public static void LogWarningEditor(this string message)
		{
#if UNITY_EDITOR
			Debug.LogWarning(message);
#endif
		}
		
		public static void LogError(this string message)
		{
			Debug.LogError(message);
		}

		public static void LogErrorEditor(this string message)
		{
#if UNITY_EDITOR
			Debug.LogError(message);
#endif
		}
	}
}