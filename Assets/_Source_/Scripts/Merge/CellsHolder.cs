namespace miniit.MERGE
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using Random = UnityEngine.Random;

	public class CellsHolder : IDisposable
	{
		private readonly List<GridCell> emptyCells;
		private readonly List<GridCell> occupiedCells;

		public CellsHolder(List<GridCell> cells)
		{
			if (Instance != null)
			{
				Debug.LogWarning("Cannot initialize more than once");

				return;
			}

			emptyCells = cells;
			occupiedCells = new List<GridCell>(cells.Count);

			Instance = this;
		}

		public static CellsHolder Instance { get; private set; }

		public bool CanOccupyCell => emptyCells.Count > 0;

		public void Dispose()
		{
			Instance = null;
		}

		public bool TryOccupyCell(MergeItem mergeItem)
		{
			if (CanOccupyCell)
			{
				int randomIndex = Random.Range(0, emptyCells.Count);

				GridCell cell = emptyCells[randomIndex];
				cell.SetItem(mergeItem);
				emptyCells.RemoveAt(randomIndex);
				occupiedCells.Add(cell);
				return true;
			}

			return false;
		}

		public void ReleaseCell(GridCell cell)
		{
			emptyCells.Add(cell);
		}
	}
}