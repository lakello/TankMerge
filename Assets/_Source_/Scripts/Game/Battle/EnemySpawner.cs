namespace MiniIT.GAME.Battle
{
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using Entities;
    using MERGE.UNIT;
    using Settings;
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
        private TankConfig              config;

        private void Start()
        {
            config = GlobalData.Container.Resolve<TankConfig>();
            battleTargetHolder = GlobalData.Container.Resolve<BattleTargetHolder>();
            battleTargetHolder.UnitAdded += OnUnitRemoved;
            battleTargetHolder.UnitRemoved += OnUnitRemoved;
            SpawnUnits(ctx.Token).Forget();
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
            SpawnUnits(ctx.Token).Forget();
        }

        private async UniTaskVoid SpawnUnits(CancellationToken token)
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
                if (Chance.Default(spawnChance) == false)
                {
                    continue;
                }

                battleTargetHolder.TryAddUnit(
                    Fraction.Enemy,
                    () =>
                    {
                        int level = Random.Range(1, MergeUnit.MaxLevel + 1);
                        Tank tank = config.GetTankByLevel(level);

                        tank.Init(new BattleData
                        {
                            SelfFraction = Fraction.Enemy,
                            TargetFraction = Fraction.Friends,
                            MaxHealth = 100
                        });

                        return new TankSpawnData
                        {
                            Tank = tank,
                            Level = level,
                            Removed = data => config.ReleaseTank(data.Tank, data.Level)
                        };
                    });
            }
        }
    }
}