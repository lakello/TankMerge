namespace MiniIT.GAME.Battle
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public class Bullet : IDisposable
    {
        private IBulletBehaviour behaviour;
        private Health           targetHealth;

        private CancellationTokenSource ctx = null;

        public Bullet(IBulletBehaviour bulletBehaviour, Health health)
        {
            behaviour = bulletBehaviour;
            targetHealth = health;
            ctx = new CancellationTokenSource();

            UpdateBehaviour(ctx.Token).Forget();
        }

        public void Dispose()
        {
            ctx?.Cancel();
            
            behaviour = null;
            targetHealth = null;
            ctx = null;
        }

        private async UniTaskVoid UpdateBehaviour(CancellationToken token)
        {
            behaviour.Init(targetHealth);

            while (token.IsCancellationRequested == false)
            {
                if (behaviour.TryDealDamage() == false)
                {
                    behaviour.Move();
                }
                else
                {
                    Dispose();
                }

                await UniTask.Yield();
            }
        }
    }
}