namespace miniit.GAME.MERGE.CELL
{
	using System;
	using System.Collections.Generic;
	using ITEM;
	using UnityEngine;
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

		public bool CanOccupyCell => cells.AsValueEnumerable().Any(c => c.Item == null);

		public void Dispose()
		{
			Instance = null;
		}

		public bool TryOccupyRandomCell(MergeItem mergeItem)
		{
			var emptyCells = cells.AsValueEnumerable().Where(c => c.Item == null).ToArray();

			if (emptyCells.Length > 0)
			{
				int randomIndex = Random.Range(0, emptyCells.Length);

				GridCell cell = emptyCells[randomIndex];
				cell.SetItem(mergeItem);
				return true;
			}

			return false;
		}

		public void ReleaseCell(GridCell cell)
		{
			cell.Release();
			cells.Add(cell);
		}
	}
}