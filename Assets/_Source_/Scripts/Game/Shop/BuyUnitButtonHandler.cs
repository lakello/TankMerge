namespace MiniIT.GAME
{
    using Doozy.Runtime.UIManager;
    using Doozy.Runtime.UIManager.Components;
    using MERGE;
    using MERGE.CELL;
    using PLAYER;
    using UnityEngine;
    using UtilsModule.Other;

    public class BuyUnitButtonHandler : MonoBehaviour
    {
        [SerializeField] private UIButton  button;
        [SerializeField] private MergeUnitSpawner spawner;
        [SerializeField] private PriceView priceView;

        private Wallet wallet = null;

        private void Awake()
        {
            wallet = GlobalData.Container.Resolve<Wallet>();
        }

        private void OnEnable()
        {
            button.OnSelectionStateChangedCallback.AddListener(OnClick);
        }

        private void OnDisable()
        {
            button.OnSelectionStateChangedCallback.RemoveListener(OnClick);
        }

        private void OnClick(UISelectionState selectionState)
        {
            if (selectionState == UISelectionState.Pressed)
            {
                if (CellsHolder.Instance.CanOccupyCell && wallet.TryBuy(PriceHolder.Instance.Price))
                {
                    PriceHolder.Instance.NextPrice();
                    priceView.UpdateView();
                    spawner.Spawn();
                }
            }
        }
    }
}