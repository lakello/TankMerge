namespace MiniIT.GAME.MERGE.CELL
{
    using System;
    using UNIT;
    using UnityEngine;

    public class GridCell : MonoBehaviour
    {
        [SerializeField] private Transform placePoint;

        private MergeUnit mergeUnit;

        public event Action<GridCell> Changed = delegate { };

        public MergeUnit MergeUnit => mergeUnit;

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
            if (this.mergeUnit == null)
            {
                this.mergeUnit = unit;
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