namespace MiniIT.GAME.MERGE
{
    using CELL;
    using MessageModule;
    using Messages;
    using UNIT;
    using UnityEngine;

    public class MergeHandler : MonoBehaviour
    {
        [SerializeField] private DragHandler dragHandler;
        [SerializeField] private float       castDistance = 2.0f;

        private void OnEnable()
        {
            dragHandler.DragEnded += OnDragEnded;
        }

        private void OnDisable()
        {
            dragHandler.DragEnded -= OnDragEnded;
        }

        private void OnDragEnded(GridCell currentCell)
        {
            GridCell nearestCell = GridCell.GetNearestCell(currentCell, castDistance);

            if (nearestCell != null)
            {
                CheckCell();
                nearestCell.UpdateView(null, null);
            }
            else
            {
                currentCell.ResetItemToPlace();
            }
            
            currentCell.UpdateView(null, null);

            return;

            void CheckCell()
            {
                if (nearestCell.MergeUnit == null)
                {
                    nearestCell.SetItem(currentCell.MergeUnit);
                    CellsHolder.Instance.ReleaseCell(currentCell);
                }
                else
                {
                    if (nearestCell.MergeUnit == currentCell.MergeUnit)
                    {
                        currentCell.ResetItemToPlace();
                    }
                    else
                    {
                        TryMerge();
                    }
                }
            }

            void TryMerge()
            {
                if (nearestCell.MergeUnit.CurrentLevel == currentCell.MergeUnit.CurrentLevel)
                {
                    int currentLevel = currentCell.MergeUnit.CurrentLevel;

                    MergeUnit.Pool.Release(nearestCell.MergeUnit);
                    MergeUnit.Pool.Release(currentCell.MergeUnit);

                    CellsHolder.Instance.ReleaseCell(currentCell);
                    CellsHolder.Instance.ReleaseCell(nearestCell);

                    new MergeSuccessMessage
                    {
                        TargetCell = nearestCell,
                        Level = currentLevel,
                    }.Publish();
                }
                else
                {
                    currentCell.ResetItemToPlace();
                }
            }
        }
    }
}