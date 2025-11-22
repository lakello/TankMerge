namespace MiniIT.GAME.PLAYER
{
    using R3;
    using TMPro;
    using UnityEngine;
    using UtilsModule.Other;

    public class WalletView : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyOut;

        private Wallet              wallet     = null;
        private CompositeDisposable disposable = null;

        private void Awake()
        {
            wallet = GlobalData.Container.Resolve<Wallet>();
        }

        private void OnEnable()
        {
            disposable = new CompositeDisposable();
            wallet.Money
                .Subscribe(v => moneyOut.text = v.ToString())
                .AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable?.Dispose();
        }
    }
}