using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UtilsModule.Other;

namespace UtilsModule.Disposable
{
	public class SceneDisposableHolder : MonoBehaviour
	{
		private static Dictionary<string, SceneDisposableHolder> s_holders = new Dictionary<string, SceneDisposableHolder>();

		private readonly List<IDisposable> _disposables = new List<IDisposable>();
		private string _sceneName;

		public static void Create(string sceneName)
		{
			if (s_holders.ContainsKey(sceneName))
			{
				throw new Exception($"There is already an Instance of {nameof(SceneDisposableHolder)}");
			}
			
			var instance = Factory.CreateGameObject<SceneDisposableHolder>();
			instance._sceneName = sceneName;
			s_holders.Add(sceneName, instance);

			SceneManager.MoveGameObjectToScene(instance.gameObject, SceneManager.GetSceneByName(sceneName.ToString()));
		}

		private void OnDestroy()
		{
			_disposables.ForEach(disposable => disposable?.Dispose());
			s_holders.Remove(_sceneName);
		}

		public static void Register(IDisposable disposable, string sceneName)
		{
			if (s_holders.TryGetValue(sceneName, out var holder))
			{
				holder._disposables.Add(disposable);
			}
			else
			{
				throw new ArgumentException($"{nameof(SceneDisposableHolder)} in {sceneName} not instance");
			}
		}
	}
}