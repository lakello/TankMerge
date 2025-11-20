namespace miniit.GAME.MERGE
{
	using System.Collections.Generic;
	using MERGE.CELL;
	using Sirenix.OdinInspector;
	using UnityEditor;
	using UnityEngine;
	using UtilsModule.Execute;
	using UtilsModule.Execute.Interfaces;
	using UtilsModule.Extensions;

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
		[ReadOnly]
		[SerializeField]
		private List<GridCell> cells;

		public ExecuteMethod Method => ExecuteMethod.Start;
		public int Priority { get; set; }

		public Executor GetExecutor()
		{
			return new ExecutorSync(Init);
		}

		private void Init()
		{
			new CellsHolder(cells);
		}

#if UNITY_EDITOR
		[Button]
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

			cells = new List<GridCell>();

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

					GridCell cellInstance = PrefabUtility.InstantiatePrefab(cellPrefab, gridCenter) as GridCell;
					cellInstance.transform.position = cellPosition;
					cells.Add(cellInstance);
				}
			}
			
			this.SetDirty();
		}

		[Button]
		private void Clear()
		{
			if (cells is { Count: > 0 })
			{
				for (int i = 0; i < cells.Count; i++)
				{
					DestroyImmediate(cells[i].gameObject);
				}
				
				cells.Clear();
			}
		}
#endif
		
	}
}