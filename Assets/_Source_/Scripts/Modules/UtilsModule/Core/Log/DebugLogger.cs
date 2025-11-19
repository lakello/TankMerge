namespace _GameResources.Scripts.Modules.UtilsModule.Core.Log
{
	using UnityEngine;

	public class DebugLogger
	{
		private string _prefix;

		public DebugLogger(string prefix)
		{
			_prefix = prefix;
		}
		
		public void Send(string message)
		{
			Debug.Log($"[{_prefix}] {message}");
		}
		
		public void SendWarning(string message)
		{
			Debug.LogWarning($"[{_prefix}] {message}");
		}
		
		public void SendError(string message)
		{
			Debug.LogError($"[{_prefix}] {message}");
		}
	}
}