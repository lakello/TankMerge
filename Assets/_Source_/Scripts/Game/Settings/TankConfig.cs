namespace MiniIT.GAME.Settings
{
    using System.Collections.Generic;
    using Entities;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Pool;

    [CreateAssetMenu(menuName = "Game Settings/UnitConfig")]
    public class TankConfig : SerializedScriptableObject
    {
        [SerializeField] private Tank[]     tanks;
        [SerializeField] private Material[] skins;

        private Dictionary<int, ObjectPool<Tank>> pools;
        private Transform                         parent;

        public Tank GetTankByLevel(int level)
        {
            pools ??= new Dictionary<int, ObjectPool<Tank>>();
            parent ??= new GameObject($"{nameof(Tank)}:POOL").transform;

            int index = GetIndexByLevel(tanks.Length, level);

            if (pools.ContainsKey(index) == false)
            {
                pools.Add(index, new ObjectPool<Tank>(
                    createFunc: () => Instantiate(tanks[index], parent),
                    actionOnGet: tank => tank.gameObject.SetActive(true),
                    actionOnRelease: tank =>
                    {
                        tank.transform.SetParent(parent);
                        tank.gameObject.SetActive(false);
                    }));
            }

            return pools[index].Get();
        }

        public void ReleaseTank(Tank tank, int level)
        {
            int index = GetIndexByLevel(tanks.Length, level);

            if (pools.ContainsKey(index) == false)
            {
                return;
            }

            pools[index].Release(tank);
        }

        public Material GetSkinByLevel(int level)
        {
            return skins[GetIndexByLevel(skins.Length, level)];
        }

        private T GetDataByLevel<T>(T[] data, int level)
        {
            int index = Mathf.Max(0, level - 1);

            if (index < data.Length)
            {
                return data[index];
            }

            return data[data.Length % index];
        }

        private int GetIndexByLevel(int length, int level)
        {
            int index = Mathf.Max(0, level - 1);

            if (index < length)
            {
                return index;
            }

            return length % index;
        }
    }
}