using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UtilsModule.Disposable
{
	public static class SceneParent
	{
		private static Dictionary<string, Transform> _sceneParents = new Dictionary<string, Transform>();

		public static void SetParentInScene(this Transform child, string sceneName)
		{
			_sceneParents.TryAdd(sceneName, null);

			if (_sceneParents[sceneName] == null)
			{
				var obj = new GameObject(nameof(SceneParent));
				_sceneParents[sceneName] = obj.transform;
				SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName(sceneName));
			}

			child.SetParent(_sceneParents[sceneName]);
		}
	}
}