namespace MiniIT.GAME.PLAYER
{
    using System;
    using R3;

    public class Wallet : IDisposable
    {
        private const int START_VALUE = 0;

        public ReactiveProperty<int> Money { get; } = new ReactiveProperty<int>(START_VALUE);

        public bool TryBuy(int price)
        {
            if (Money.Value >= price)
            {
                Money.Value -= price;
                return true;
            }

            return false;
        }

        public void AddMoney(int value)
        {
            if (value > 0)
            {
                Money.Value += value;
            }
        }

        public void Dispose()
        {
            Money.Dispose();
        }
    }
}