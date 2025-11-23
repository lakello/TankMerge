namespace MiniIT.GAME
{
    using System;
    using System.Threading;
    using AudioModule;
    using AudioModule.Extensions;
    using Cysharp.Threading.Tasks;
    using MERGE.CELL;
    using MERGE.UNIT;
    using miniit.INPUT;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UtilsModule.Other;

    public class DragHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float     holdHeight;
        [SerializeField] private float     castDistance = 2f;

        private InputActions       inputActions = null;
        private UnityEngine.Camera camera       = null;

        private GridCell                currentCell = null;
        private CancellationTokenSource ctx         = null;

        public event Action<GridCell> DragEnded = delegate { };

        private void Awake()
        {
            inputActions = GlobalData.Container.Resolve<InputActions>();
            camera = UnityEngine.Camera.main;
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

                if (gridCell.MergeUnit == null)
                {
                    return;
                }

                currentCell = gridCell;

                ctx = new CancellationTokenSource();
                DragItem(ctx.Token).Forget();
                AudioID.PickUp.PlayOneShot();
            }
        }

        private void OnClickCancelled(InputAction.CallbackContext _)
        {
            if (currentCell == null)
            {
                return;
            }
            
            AudioID.PickDown.PlayOneShot();

            DragEnded(currentCell);

            ctx?.Cancel();
        }

        private async UniTaskVoid DragItem(CancellationToken token)
        {
            UpdateSelected(token).Forget();

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

                MergeUnit unit = currentCell.MergeUnit;
                if (unit == null)
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

                    Vector3 localPointOffset = unit.transform.position - unit.GrabPoint.position;
                    Vector3 targetPosition = hitPoint + localPointOffset;

                    unit.transform.position = targetPosition;
                }

                await UniTask.Yield();
            }

            currentCell = null;
        }

        private async UniTaskVoid UpdateSelected(CancellationToken token)
        {
            GridCell previousCell = null;

            while (token.IsCancellationRequested == false)
            {
                if (currentCell == null)
                {
                    return;
                }

                GridCell nearestCell = GridCell.GetNearestCell(currentCell, castDistance);

                if (previousCell != nearestCell)
                {
                    if (previousCell != null)
                    {
                        previousCell.UpdateView(nearestCell, currentCell);
                    }

                    if (nearestCell != null)
                    {
                        nearestCell.UpdateView(nearestCell, currentCell);
                    }

                    previousCell = nearestCell;
                }

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            }
        }
    }
}