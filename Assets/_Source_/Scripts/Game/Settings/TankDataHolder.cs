namespace MiniIT.GAME.Settings
{
    using System.Collections.Generic;
    using Entities;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Pool;

    [CreateAssetMenu(menuName = "Game Settings/UnitConfig")]
    public class TankDataHolder : SerializedScriptableObject
    {
        [BoxGroup("Damage")] [SerializeField] private MultiplierData damageMultiplier;
        [BoxGroup("Health")] [SerializeField] private MultiplierData healthMultiplier;

        [BoxGroup("Reward")] [SerializeField] private MultiplierData takeDamageRewardMultiplier;
        [BoxGroup("Reward")] [SerializeField] private MultiplierData deadRewardMultiplier;

        [BoxGroup("General")] [SerializeField] private Vector2 levelRange;
        [BoxGroup("General")] [SerializeField] private Tank[]  tanks;

        private Dictionary<int, ObjectPool<Tank>> pools  = null;
        private Transform                         parent = null;

        public Tank GetTankByLevel(int level)
        {
            pools ??= new Dictionary<int, ObjectPool<Tank>>();
            parent ??= new GameObject($"{nameof(Tank)}:POOL").transform;

            int index = GetIndexByLevel(tanks.Length, level);

            if (pools.ContainsKey(index) == false)
            {
                pools.Add(index, new ObjectPool<Tank>(
                    createFunc: () => Instantiate(tanks[index], parent),
                    actionOnGet: tank =>
                    {
                        tank.Init(new TankLevelConfig
                        {
                            Level = level,
                            DamageMultiplier = damageMultiplier.Get(level, levelRange),
                            HealthMultiplier = healthMultiplier.Get(level, levelRange),
                            TakeDamageRewardMultiplier = takeDamageRewardMultiplier.Get(level, levelRange),
                            DeadRewardMultiplier = deadRewardMultiplier.Get(level, levelRange),
                        });
                        tank.gameObject.SetActive(true);
                    },
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