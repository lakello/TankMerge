namespace MiniIT.GAME.BATTLE
{
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using ENTITIES;
    using MERGE;
    using SETTINGS;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UtilsModule.Other;
    using Random = UnityEngine.Random;

    public class EnemySpawner : MonoBehaviour
    {
        [Range(0, 100)] [SerializeField] private float spawnChance;
        [SerializeField]                 private float cooldown;

        private CancellationTokenSource ctx = new CancellationTokenSource();
        private BattleTargetHolder      battleTargetHolder;
        private TankDataHolder          dataHolder;

        private void Start()
        {
            dataHolder = GlobalData.Container.Resolve<TankDataHolder>();
            battleTargetHolder = GlobalData.Container.Resolve<BattleTargetHolder>();
            battleTargetHolder.UnitAdded += OnUnitRemoved;
            battleTargetHolder.UnitRemoved += OnUnitRemoved;
            SpawnUnits(100, ctx.Token).Forget();
        }

        private void OnDestroy()
        {
            battleTargetHolder.UnitAdded -= OnUnitRemoved;
            battleTargetHolder.UnitRemoved -= OnUnitRemoved;
            ctx.Cancel();
        }

        [Button]
        private void OnUnitRemoved(Fraction fraction)
        {
            SpawnUnits(battleTargetHolder.IsAllEmpty(Fraction.Enemy)
                ? 100
                : spawnChance, ctx.Token).Forget();
        }

        private async UniTaskVoid SpawnUnits(float chance, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            await UniTask.WaitForSeconds(cooldown, cancellationToken: token);

            if (token.IsCancellationRequested)
            {
                return;
            }

            int maxCount = battleTargetHolder.GetEmptyCount(Fraction.Enemy);

            if (maxCount == 0)
            {
                return;
            }

            int count = Random.Range(1, maxCount);

            for (int i = 0; i < count; i++)
            {
                if (Chance.Default(chance) == false)
                {
                    continue;
                }

                battleTargetHolder.TryAddUnit(
                    Fraction.Enemy,
                    () =>
                    {
                        int level = Random.Range(1, MergeUnit.MaxLevel + 1);
                        Tank tank = dataHolder.GetTankByLevel(level);

                        tank.Run(new BattleData
                        {
                            SelfFraction = Fraction.Enemy,
                            TargetFraction = Fraction.Friends,
                        });

                        return new TankSpawnData
                        {
                            Tank = tank,
                            Level = level,
                            Removed = data => dataHolder.ReleaseTank(data.Tank, data.Level)
                        };
                    });
            }
        }
    }
}