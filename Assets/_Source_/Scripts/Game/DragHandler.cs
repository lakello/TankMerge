namespace miniit.GAME
{
	using System;
	using System.Threading;
	using Cysharp.Threading.Tasks;
	using INPUT;
	using MERGE.CELL;
	using UnityEngine;
	using UnityEngine.InputSystem;
	using UtilsModule.Other;

	public class DragHandler : MonoBehaviour
	{
		[SerializeField]
		private LayerMask layerMask;
		[SerializeField]
		private float holdHeight;

		private InputActions inputActions = null;
		private Camera camera = null;

		private GridCell currentCell = null;
		private CancellationTokenSource ctx;

		public event Action<GridCell> DragEnded = delegate { };

		private void Awake()
		{
			inputActions = DI.Resolve<InputActions>();
			camera = Camera.main;
		}

		private void OnEnable()
		{
			inputActions.UI.Click.started += OnClickStarted;
			inputActions.UI.Click.canceled += OnClickCancelled;
		}

		private void OnDisable()
		{
			inputActions.UI.Click.started -= OnClickStarted;
			inputActions.UI.Click.canceled -= OnClickCancelled;
			ctx?.Cancel();
		}

		private void OnClickStarted(InputAction.CallbackContext _)
		{
			if (camera == null)
			{
				return;
			}

			Vector2 mousePosition = Mouse.current.position.ReadValue();

			Ray ray = camera.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, 0.0f));
			RaycastHit hitInfo;

			if (Physics.Raycast(ray, out hitInfo, 100.0f, layerMask))
			{
				GridCell gridCell = hitInfo.collider.GetComponent<GridCell>();

				if (gridCell == null)
				{
					return;
				}

				if (gridCell.Item == null)
				{
					return;
				}

				currentCell = gridCell;

				Vector3 position = currentCell.Item.transform.position;
				position.y = holdHeight;
				currentCell.Item.transform.position = position;

				ctx = new CancellationTokenSource();

				DragItem(ctx.Token).Forget();
			}
		}

		private void OnClickCancelled(InputAction.CallbackContext _)
		{
			if (currentCell == null)
			{
				return;
			}

			DragEnded(currentCell);

			ctx?.Cancel();
		}

		private async UniTaskVoid DragItem(CancellationToken token)
		{
			while (token.IsCancellationRequested == false)
			{
				if (currentCell == null)
				{
					return;
				}

				if (camera == null)
				{
					return;
				}

				Vector2 mousePosition = Mouse.current.position.ReadValue();
				Ray ray = camera.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, 0.0f));

				Plane plane = new Plane(Vector3.up, new Vector3(0.0f, holdHeight, 0.0f));

				float enter;
				if (plane.Raycast(ray, out enter))
				{
					Vector3 hitPoint = ray.GetPoint(enter);
					currentCell.Item.transform.position = hitPoint;
				}

				await UniTask.Yield();
			}

			currentCell = null;
		}
	}
}