namespace MiniIT.GAME.Battle
{
    using Entities;
    using MERGE.CELL;
    using MERGE.UNIT;
    using Settings;
    using UnityEngine;
    using UtilsModule.Other;
    using ZLinq;

    public class FriendsWrapper : MonoBehaviour
    {
        [SerializeField] private GridCell[] cells;

        private BattleTargetHolder battleTargetHolder;
        private TankConfig         tankConfig;

        private void Awake()
        {
            tankConfig = GlobalData.Container.Resolve<TankConfig>();
        }

        private void OnEnable()
        {
            battleTargetHolder ??= GlobalData.Container.Resolve<BattleTargetHolder>();

            battleTargetHolder.UnitRemoved += OnUnitRemoved;

            foreach (GridCell gridCell in cells)
            {
                gridCell.Changed += OnCellChanged;
            }
        }

        private void OnDisable()
        {
            battleTargetHolder.UnitRemoved -= OnUnitRemoved;

            foreach (GridCell gridCell in cells)
            {
                gridCell.Changed -= OnCellChanged;
            }
        }

        private void OnCellChanged(GridCell cell)
        {
            if (cell.MergeUnit != null)
            {
                TryWrap(cell);
            }
        }

        private void TryWrap(GridCell cell)
        {
            if (battleTargetHolder.TryAddUnit(
                Fraction.Friends,
                () =>
                {
                    Tank tank = tankConfig.GetTankByLevel(cell.MergeUnit.CurrentLevel);

                    tank.Init(new BattleData
                    {
                        SelfFraction = Fraction.Friends,
                        TargetFraction = Fraction.Enemy,
                        MaxHealth = 100
                    });

                    return new TankSpawnData
                    {
                        Tank = tank,
                        Level = cell.MergeUnit.CurrentLevel,
                        Removed = data => tankConfig.ReleaseTank(data.Tank, data.Level)
                    };
                }))
            {
                MergeUnit.Pool.Release(cell.MergeUnit);
                cell.Release();
            }
        }

        private void OnUnitRemoved(Fraction fraction)
        {
            if (fraction == Fraction.Friends)
            {
                GridCell cell = cells.AsValueEnumerable().FirstOrDefault(c => c.MergeUnit != null);

                if (cell != null)
                {
                    TryWrap(cell);
                }
            }
        }
    }
}