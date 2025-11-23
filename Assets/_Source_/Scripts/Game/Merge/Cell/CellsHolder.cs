namespace MiniIT.GAME.MERGE
{
    using System;
    using System.Collections.Generic;
    using SHOP;
    using UnityEngine;
    using UtilsModule.Other;
    using ZLinq;
    using Random = UnityEngine.Random;

    public class CellsHolder : IDisposable
    {
        private readonly List<GridCell> cells;

        public CellsHolder(List<GridCell> cells)
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot initialize more than once");

                return;
            }

            this.cells = cells;

            Instance = this;
        }

        public static CellsHolder Instance { get; private set; }

        public bool CanOccupyCell => cells.AsValueEnumerable().Any(c => c.MergeUnit == null);

        public void Dispose()
        {
            Instance = null;
        }

        public bool TryOccupyRandomCell(MergeUnit mergeUnit)
        {
            var emptyCells = cells.AsValueEnumerable().Where(c => c.MergeUnit == null).ToArray();

            if (emptyCells.Length > 0)
            {
                int randomIndex = Random.Range(0, emptyCells.Length);

                GridCell cell = emptyCells[randomIndex];
                cell.SetItem(mergeUnit);

                if (GlobalData.Container.TryResolve(out PriceView view))
                {
                    view.UpdateView();
                }

                return true;
            }

            return false;
        }

        public void ReleaseCell(GridCell cell)
        {
            cell.Release();

            if (GlobalData.Container.TryResolve(out PriceView view))
            {
                view.UpdateView();
            }
        }
    }
}