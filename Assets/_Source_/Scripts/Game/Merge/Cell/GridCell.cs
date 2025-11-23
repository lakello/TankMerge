namespace MiniIT.GAME.MERGE
{
    using System;
    using UnityEngine;

    public class GridCell : MonoBehaviour
    {
        private const int HITS_COUNT = 4;

        private static readonly RaycastHit[] hitsCache = new RaycastHit[HITS_COUNT];
        private static readonly int          BaseColor = Shader.PropertyToID("_BaseColor");

        [SerializeField] private LayerMask    layerMask;
        [SerializeField] private Transform    placePoint;
        [SerializeField] private MeshRenderer borderRenderer;
        [SerializeField] private Color        defaultColor;
        [SerializeField] private Color        emptyColor;
        [SerializeField] private Color        blockColor;

        private MergeUnit mergeUnit;

        public event Action<GridCell> Changed = delegate { };

        public MergeUnit MergeUnit => mergeUnit;

        public static GridCell GetNearestCell(GridCell currentCell, float castDistance)
        {
            if (currentCell == null || currentCell.MergeUnit == null)
            {
                return null;
            }

            MergeUnit unit = currentCell.MergeUnit;

            Transform itemTransform = unit.transform;

            BoxCollider boxCollider = null;
            bool hasCollider = unit.TryGetComponent(out boxCollider);
            if (!hasCollider || boxCollider == null)
            {
                return null;
            }

            Vector3 halfExtents = boxCollider.bounds.extents;

            Vector3 origin = itemTransform.position;
            origin.y += halfExtents.y;

            int hitCount = Physics.BoxCastNonAlloc(
                origin,
                halfExtents,
                Vector3.down,
                hitsCache,
                Quaternion.identity,
                castDistance,
                currentCell.layerMask
            );

            if (hitCount == 0)
            {
                return null;
            }

            GridCell nearestCell = null;

            for (int i = 0; i < hitCount; i++)
            {
                if (hitsCache[i].collider.TryGetComponent(out GridCell cell))
                {
                    if (nearestCell == null
                        || Vector3.Distance(origin, nearestCell.transform.position) > Vector3.Distance(origin, cell.transform.position))
                    {
                        nearestCell = cell;
                    }
                }
            }

            return nearestCell;
        }

        public void UpdateView(GridCell nearestCell, GridCell sourceCell)
        {
            if (nearestCell == this)
            {
                if (mergeUnit == null || nearestCell.MergeUnit.CurrentLevel == sourceCell.MergeUnit.CurrentLevel)
                {
                    borderRenderer.material.SetColor(BaseColor, emptyColor);
                }
                else
                {
                    borderRenderer.material.SetColor(BaseColor, blockColor);
                }
            }
            else
            {
                borderRenderer.material.SetColor(BaseColor, defaultColor);
            }
        }

        public void ResetItemToPlace()
        {
            if (mergeUnit == null)
            {
                return;
            }

            mergeUnit.transform.position = placePoint.position;
            mergeUnit.transform.rotation = placePoint.rotation;
        }

        public void SetItem(MergeUnit unit)
        {
            if (mergeUnit == null)
            {
                mergeUnit = unit;
                ResetItemToPlace();

                Changed(this);
                return;
            }

            throw new Exception("Cannot set more than once");
        }

        public void Release()
        {
            mergeUnit = null;
            Changed(this);
        }
    }
}