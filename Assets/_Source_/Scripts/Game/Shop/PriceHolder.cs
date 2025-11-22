namespace MiniIT.GAME
{
    using UnityEngine;
    using UtilsModule.Singleton;

    public class PriceHolder : SingletonMono<PriceHolder>
    {
        [SerializeField] private int   startPrice;
        [SerializeField] private float priceMultiplier;

        public int Price { get; private set; }

        public void NextPrice()
        {
            if (Price == 0)
            {
                Price = startPrice;
            }
            else
            {
                Price = (int)(Price * priceMultiplier);
            }
        }
    }
}