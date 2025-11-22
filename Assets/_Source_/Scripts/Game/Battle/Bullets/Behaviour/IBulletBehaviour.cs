namespace MiniIT.GAME.Battle
{
    public interface IBulletBehaviour
    {
        public void Init(BulletBehaviourData data);

        public void Move();

        public bool TryDealDamage();
    }
}