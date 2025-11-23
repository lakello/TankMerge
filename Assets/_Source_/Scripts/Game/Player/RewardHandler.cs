namespace MiniIT.GAME.PLAYER
{
    using System;
    using BATTLE;
    using MessageModule;
    using R3;
    using UtilsModule.Other;

    public class RewardHandler : IDisposable
    {
        private readonly CompositeDisposable disposable = new CompositeDisposable();
        private readonly Wallet              wallet;

        public RewardHandler()
        {
            wallet = GlobalData.Container.Resolve<Wallet>();

            new DamageTakedMessage().Receive()
                .Where(m => m.SelfFraction == Fraction.Enemy)
                .Subscribe(OnDamageTaked)
                .AddTo(disposable);
        }

        public void Dispose()
        {
            disposable.Dispose();
        }

        private void OnDamageTaked(DamageTakedMessage message)
        {
            wallet.AddMoney(message.IsAlive
                ? message.TakeDamageReward
                : message.DeadReward);
        }
    }
}