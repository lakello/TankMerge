using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UtilsModule.Execute;
using UtilsModule.Execute.Interfaces;
using UtilsModule.Other;

namespace UtilsModule.Singleton
{
	public enum SingletonMode
	{
		Global,
		Scene,
	}

	public abstract class SingletonMono<T> : SerializedMonoBehaviour, IExecuteHolder
		where T : SingletonMono<T>
	{
		public static T Instance { get; private set; }

		[SerializeField] private SingletonMode _mode;

		private bool _created;

		public virtual ExecuteMethod Method => ExecuteMethod.Awake;
		public int Priority { get; set; }
		
		public Executor GetExecutor()
		{
			return new ExecutorSync(Init);
		}

		private void OnDestroy()
		{
			Instance = null;
		}

		protected static void Create(SingletonMode mode, string sceneName = null)
		{
			if (Instance != null)
			{
				throw new Exception($"There is already an Instance of {nameof(T)}");
			}
			
			Instance = Factory.CreateGameObject<T>();
			Instance._created = true;
			Instance._mode = mode;

			if (mode == SingletonMode.Scene && string.IsNullOrEmpty(sceneName) == false)
			{
				SceneManager.MoveGameObjectToScene(Instance.gameObject, SceneManager.GetSceneByName(sceneName));
			}
			
			Instance.Init();
		}

		private void Init()
		{
			if (_created)
			{
				CheckMode();

				return;
			}

			if (Instance == null)
			{
				Instance = this as T;
				CheckMode();
			}
			else
			{
				Destroy(gameObject);
			}

			return;

			void CheckMode()
			{
				switch (_mode)
				{
					case SingletonMode.Global:
						DontDestroyOnLoad(gameObject);

						break;
					case SingletonMode.Scene:
						break;
				}
			}
		}
	}
	
	public abstract class Singleton<T>
		where T : Singleton<T>, new()
	{
		public static T Instance { get; private set; }
		
		public static void Create()
		{
			if (Instance != null)
			{
				throw new Exception($"There is already an Instance of {nameof(T)}");
			}

			Instance = new T();
		}
	}
	
	public abstract class Singleton<T, TReadOnly>
		where T : Singleton<T, TReadOnly>, TReadOnly, new()
	{
		public static TReadOnly Instance { get; private set; }
		
		public static T Create()
		{
			if (Instance != null)
			{
				throw new Exception($"There is already an Instance of {nameof(T)}");
			}

			var instance = new T();
			Instance = instance;
			
			return instance;
		}
	}
}