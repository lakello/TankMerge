using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UtilsModule.Extensions
{
	public static class GameObjectExtension
	{
#if UNITY_EDITOR
		public static bool IsPrefabInScene(this GameObject origin)
		{
			PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
			bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
			bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(origin) == PrefabInstanceStatus.Connected;

			return !isValidPrefabStage && prefabConnected;
		}

		public static bool TryGetPrefabReference(this GameObject obj, out AssetReference asset, string groupName = "")
		{
			asset = null;

			if (obj == null)
				return false;

			// 1. Проверяем: является ли объект вообще связанным с префабом
			bool isPrefabInstance = PrefabUtility.IsPartOfPrefabInstance(obj);
			bool isPrefabAsset = PrefabUtility.IsPartOfPrefabAsset(obj);

			if (!isPrefabInstance && !isPrefabAsset)
				return false; // Не префаб и не инстанс → выходим

			// 2. Если это инстанс на сцене — получаем исходный префаб-ассет
			if (isPrefabInstance)
			{
				GameObject prefabAsset = PrefabUtility.GetCorrespondingObjectFromSource(obj);
				if (prefabAsset == null)
					return false;
				obj = prefabAsset;
				// После этого obj должен быть ассетом
			}

			// Теперь obj должен быть префаб-ассетом в Project
			string assetPath = AssetDatabase.GetAssetPath(obj);
			if (string.IsNullOrEmpty(assetPath))
				return false;

			// Убедимся, что это действительно asset (а не что-то другое)
			if (AssetDatabase.LoadMainAssetAtPath(assetPath) != obj)
				return false;

			string guid = AssetDatabase.AssetPathToGUID(assetPath);
			if (string.IsNullOrEmpty(guid))
				return false;

			// 3–4. Проверяем, зарегистрирован ли уже как Addressable
			AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
			if (settings == null)
			{
				Debug.LogError("AddressableAssetSettings not found.");
				return false;
			}

			AddressableAssetEntry entry = settings.FindAssetEntry(guid);
			if (entry != null)
			{
				asset = new AssetReference(guid);
				return true;
			}

			// 5. Делаем префаб Addressable
			
			AddressableAssetGroup defaultGroup = null;

			if (string.IsNullOrEmpty(groupName) == false)
			{
				defaultGroup = settings.FindGroup(groupName);
			}
			
			if (defaultGroup == null)
			{
				Debug.LogError($"<{groupName}> Addressable group not found.");
				defaultGroup = settings.DefaultGroup;
			}
			
			if (defaultGroup == null)
			{
				Debug.LogError("Addressable group not found.");
				return false;
			}

			entry = settings.CreateOrMoveEntry(guid, defaultGroup);
			if (entry == null)
			{
				Debug.LogError($"Failed to create Addressable entry for {obj.name}");
				return false;
			}

			// Устанавливаем адрес (например, имя файла без расширения)
			string fileName = Path.GetFileNameWithoutExtension(assetPath);
			entry.address = fileName;

			// Помечаем настройки как изменённые
			settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryAdded, entry, true);
			AssetDatabase.SaveAssets();

			asset = new AssetReference(guid);
			return true;
		}
#endif

		public static void SetDirty(this GameObject obj)
		{
#if UNITY_EDITOR
			EditorUtility.SetDirty(obj);
#endif
		}

		public static T GetComponentElseThrow<T>(this GameObject origin) =>
			GetComponentElseThrow(origin, out T _);

		public static T GetComponentElseThrow<T>(this GameObject origin, out T component)
		{
			if (origin.TryGetComponent(out component) is false)
			{
				Debug.LogException(new NullReferenceException(), origin);
			}

			return component;
		}

		public static T[] GetComponentsElseThrow<T>(this GameObject origin) =>
			GetComponentsElseThrow(origin, out T[] _);

		public static T[] GetComponentsElseThrow<T>(this GameObject origin, out T[] components)
		{
			components = origin.GetComponents<T>();

			if (components is null)
			{
				Debug.LogException(new NullReferenceException(), origin);
			}

			return components;
		}

		public static T GetComponentInChildrenElseThrow<T>(this GameObject origin) =>
			GetComponentInChildrenElseThrow(origin, out T _);

		public static T GetComponentInChildrenElseThrow<T>(this GameObject origin, out T component)
		{
			component = origin.GetComponentInChildren<T>();

			if (component is null)
			{
				Debug.LogException(new NullReferenceException(), origin);
			}

			return component;
		}

		public static T GetComponentInParentElseThrow<T>(this GameObject origin) =>
			GetComponentInParentElseThrow(origin, out T _);

		public static T GetComponentInParentElseThrow<T>(this GameObject origin, out T component)
		{
			component = origin.GetComponentInParent<T>();

			if (component is null)
			{
				Debug.LogException(new NullReferenceException(), origin);
			}

			return component;
		}

		public static T[] GetComponentsInChildrenElseThrow<T>(this GameObject origin) =>
			GetComponentInChildrenElseThrow(origin, out T[] _);

		public static T[] GetComponentsInChildrenElseThrow<T>(this GameObject origin, out T[] components)
		{
			components = origin.GetComponentsInChildren<T>();

			if (components is null)
			{
				Debug.LogException(new NullReferenceException(), origin);
			}

			return components;
		}
	}
}