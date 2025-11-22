namespace MiniIT.GAME.Battle
{
    using System;
    using UnityEngine;
    using UnityEngine.Pool;
    using Object = UnityEngine.Object;

    [Serializable]
    public abstract class BulletBehaviour<T> : IBulletBehaviour
        where T : BulletBehaviour<T>
    {
        [SerializeField]
        private BulletView viewPrefab;

#region IBulletBehaviour Members

        public void Init(Health health)
        {
            BulletViewPoolHolder.InitPool(viewPrefab);
            InternalInit(health);
        }

        public abstract void Move();

        public abstract bool TryDealDamage();

#endregion
        
        protected abstract void InternalInit(Health health);

        protected static class BulletViewPoolHolder
        {
            private static Transform parent = null;

            public static ObjectPool<BulletView> Value { get; private set; }

            public static void InitPool(BulletView prefab)
            {
                if (Value != null)
                {
                    return;
                }
                
                parent ??= new GameObject($"{nameof(Bullet)}:POOL").transform;

                Value = new ObjectPool<BulletView>(
                    createFunc: () => Object.Instantiate(prefab, parent),
                    actionOnGet: view => view.gameObject.SetActive(true),
                    actionOnRelease: view => view.gameObject.SetActive(false));
            }
        }
    }
}