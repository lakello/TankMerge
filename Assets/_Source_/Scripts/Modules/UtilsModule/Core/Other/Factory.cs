using UnityEngine;

namespace UtilsModule.Other
{
	public class Factory
	{
		public static T CreateGameObject<T>()
			where T : Component
		{
			return new GameObject(typeof(T).Name).AddComponent<T>();
		}
		
		public static T CreateDontDestroyGameObject<T>()
			where T : Component
		{
			var obj = CreateGameObject<T>();
			Object.DontDestroyOnLoad(obj.gameObject);
			return obj;
		}
	}
}