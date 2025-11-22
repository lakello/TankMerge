namespace MiniIT.GAME.MERGE
{
    using CELL;
    using Doozy.Runtime.UIManager;
    using Doozy.Runtime.UIManager.Components;
    using UNIT;
    using MessageModule;
    using Messages;
    using R3;
    using UnityEngine;

    public class MergeUnitSpawner : MonoBehaviour
    {
        [SerializeField] private UIButton  button;

        private CompositeDisposable disposable;

        private void OnEnable()
        {
            button.OnSelectionStateChangedCallback.AddListener(OnSelectionStateChangedCallback);

            disposable = new CompositeDisposable();

            new MergeSuccessMessage().Receive()
                .Subscribe(OnMergeSuccess)
                .AddTo(disposable);
        }

        private void OnDisable()
        {
            button.OnSelectionStateChangedCallback.RemoveListener(OnSelectionStateChangedCallback);
            disposable?.Dispose();
        }

        private void OnMergeSuccess(MergeSuccessMessage mergeSuccessMessage)
        {
            mergeSuccessMessage.TargetCell.SetItem(GetItem(mergeSuccessMessage.Level + 1));
        }

        private void OnSelectionStateChangedCallback(UISelectionState state)
        {
            if (state == UISelectionState.Pressed)
            {
                if (CellsHolder.Instance.CanOccupyCell)
                {
                    CellsHolder.Instance.TryOccupyRandomCell(GetItem(1));
                }
            }
        }

        private MergeUnit GetItem(int level)
        {
            MergeUnit unit = MergeUnit.Pool.Get();
            unit.SetLevel(level);
            unit.gameObject.SetActive(true);
            return unit;
        }
    }
}