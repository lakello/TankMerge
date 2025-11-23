namespace MiniIT.GAME.SHOP
{
    using UnityEngine;
    using UtilsModule.Singleton;

    public class PriceHolder : SingletonMono<PriceHolder>
    {
        [SerializeField] private int   startPrice;
        [SerializeField] private float priceMultiplier;

        private float price;

        public int Price => (int)price;

        public void NextPrice()
        {
            if (price == 0)
            {
                price = startPrice;
            }
            else
            {
                price *= priceMultiplier;
            }
        }
    }
}