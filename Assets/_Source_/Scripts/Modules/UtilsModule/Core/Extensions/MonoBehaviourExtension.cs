using UnityEditor;
using UnityEngine;

namespace UtilsModule.Extensions
{
	public static class MonoBehaviourExtension
	{
		public static void SetDirty(this MonoBehaviour mono)
		{
#if UNITY_EDITOR
			EditorUtility.SetDirty(mono);
#endif
		}
	}
}