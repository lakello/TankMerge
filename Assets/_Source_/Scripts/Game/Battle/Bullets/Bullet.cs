namespace MiniIT.GAME.Battle
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public class Bullet : IDisposable
    {
        private IBulletBehaviour    behaviour;
        private BulletBehaviourData targetData;

        private CancellationTokenSource ctx = null;

        public Bullet(IBulletBehaviour bulletBehaviour, BulletBehaviourData data)
        {
            behaviour = bulletBehaviour;
            targetData = data;
            ctx = new CancellationTokenSource();

            UpdateBehaviour(ctx.Token).Forget();
        }

        public void Dispose()
        {
            ctx?.Cancel();

            behaviour = null;
            targetData = null;
            ctx = null;
        }

        private async UniTaskVoid UpdateBehaviour(CancellationToken token)
        {
            behaviour.Init(targetData);

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