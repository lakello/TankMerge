namespace MiniIT.GAME
{
    using Doozy.Runtime.Reactor.Targets;
    using Doozy.Runtime.UIManager.Animators;
    using MERGE.CELL;
    using PLAYER;
    using R3;
    using TMPro;
    using UnityEngine;
    using UtilsModule.Disposable;
    using UtilsModule.Other;

    public class PriceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text                  priceOut;
        [SerializeField] private ImageColorTarget          image;
        [SerializeField] private UISelectableColorAnimator animator;
        [SerializeField] private Color                     possibleColor;
        [SerializeField] private Color                     impossibleColor;

        private Wallet              wallet;
        private CompositeDisposable disposable;

        private void Awake()
        {
            wallet = GlobalData.Container.Resolve<Wallet>();
            GlobalData.Container.Register(this).DisposeOnExitScene(gameObject.scene.name);
        }

        private void OnEnable()
        {
            disposable = new CompositeDisposable();
            wallet.Money
                .Subscribe(_ => OnChanged())
                .AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable?.Dispose();
        }

        public void UpdateView()
        {
            OnChanged();
        }

        private void OnChanged()
        {
            int price = PriceHolder.Instance.Price;
            int money = wallet.Money.Value;
            image.SetColor(money >= price && CellsHolder.Instance != null && CellsHolder.Instance.CanOccupyCell
                ? possibleColor
                : impossibleColor);
            animator.UpdateSettings();
            priceOut.text = price.ToString();
        }
    }
}