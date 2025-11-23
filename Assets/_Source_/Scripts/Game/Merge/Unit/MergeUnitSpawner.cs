namespace MiniIT.GAME.MERGE
{
    using MessageModule;
    using R3;
    using UnityEngine;

    public class MergeUnitSpawner : MonoBehaviour
    {
        [SerializeField] private int levelChunkSize = 10;

        private CompositeDisposable disposable;
        private int                 givenUnitCount = 0;

        private void OnEnable()
        {
            disposable = new CompositeDisposable();

            new MergeSuccessMessage().Receive()
                .Subscribe(OnMergeSuccess)
                .AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable?.Dispose();
        }

        private void OnMergeSuccess(MergeSuccessMessage mergeSuccessMessage)
        {
            mergeSuccessMessage.TargetCell.SetItem(GetItem(mergeSuccessMessage.Level + 1, true));
        }

        public void Spawn()
        {
            if (CellsHolder.Instance.CanOccupyCell)
            {
                CellsHolder.Instance.TryOccupyRandomCell(GetItem(GetLevel(), false));
            }
        }

        private int GetLevel()
        {
            return Mathf.Max(1, (givenUnitCount / levelChunkSize) + 1);
        }

        private MergeUnit GetItem(int level, bool isMerge)
        {
            MergeUnit unit = MergeUnit.Pool.Get();
            unit.SetLevel(level);
            unit.gameObject.SetActive(true);

            if (isMerge == false)
            {
                givenUnitCount++;
            }

            return unit;
        }
    }
}