using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetLoadModule
{
	public class AssetLoadHelper
	{
		public static async UniTask Load<T>(AssetReference reference, Action<T> action)
		{
			var handle = Addressables.LoadAssetAsync<T>(reference);

			action(await handle);
			
			//handle.Release();
		}
		
		public static async UniTask LoadComponent<T>(AssetReference reference, Action<T> action)
		{
			var handle = Addressables.LoadAssetAsync<GameObject>(reference);

			var obj = await handle;

			if (obj.TryGetComponent(out T type))
			{
				action(type);
			}
			else
			{
				throw new ArgumentException();
			}
			
			//handle.Release();
		}
		
		public static async UniTask LoadGameObject(AssetReference reference, Action<GameObject> action)
		{
			var handle = Addressables.LoadAssetAsync<GameObject>(reference);

			action(await handle);
			
			//handle.Release();
		}
	}
}