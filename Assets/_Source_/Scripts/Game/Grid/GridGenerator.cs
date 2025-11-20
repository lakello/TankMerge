namespace miniit.GAME.MERGE
{
	using System.Collections.Generic;
	using MERGE.CELL;
	using UnityEngine;
	using UtilsModule.Execute;
	using UtilsModule.Execute.Interfaces;

	public class GridGenerator : MonoBehaviour, IExecuteHolder
	{
		[SerializeField]
		private Transform gridCenter;
		[SerializeField]
		private int rows;
		[SerializeField]
		private int columns;
		[SerializeField]
		private GridCell cellPrefab;
		[SerializeField]
		private float cellSize = 1.0f;
		[SerializeField]
		private Vector2 cellSpacing;

		public ExecuteMethod Method => ExecuteMethod.Start;
		public int Priority { get; set; }

		public Executor GetExecutor()
		{
			return new ExecutorSync(Generate);
		}

		private void Generate()
		{
			if (gridCenter == null)
			{
				return;
			}

			if (cellPrefab == null)
			{
				return;
			}

			if (rows <= 0 || columns <= 0)
			{
				return;
			}

			List<GridCell> cells = new List<GridCell>();

			float stepX = cellSize + cellSpacing.x;
			float stepZ = cellSize + cellSpacing.y;

			float width = (columns - 1) * stepX;
			float height = (rows - 1) * stepZ;

			Vector3 origin = gridCenter.position;
			origin.x -= width * 0.5f;
			origin.z -= height * 0.5f;

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					Vector3 cellPosition = origin;
					cellPosition.x += j * stepX;
					cellPosition.z += i * stepZ;

					GridCell cellInstance = Instantiate(cellPrefab, cellPosition, Quaternion.identity, gridCenter);
					cells.Add(cellInstance);
				}
			}

			new CellsHolder(cells);
		}
	}
}