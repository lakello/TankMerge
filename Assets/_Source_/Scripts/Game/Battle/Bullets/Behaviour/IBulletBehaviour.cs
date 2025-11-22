namespace MiniIT.GAME.Battle
{
    public interface IBulletBehaviour
    {
        public void Init(Health health);

        public void Move();

        public bool TryDealDamage();
    }
}