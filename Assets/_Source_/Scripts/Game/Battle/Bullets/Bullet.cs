namespace MiniIT.GAME.BATTLE
{
    using System;
    using System.Threading;
    using AudioModule;
    using AudioModule.Extensions;
    using Cysharp.Threading.Tasks;
    using PARTICLES;
    using UnityEngine;
    using UnityEngine.Pool;

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

            AudioID.Shot.PlayOneShot();

            while (token.IsCancellationRequested == false)
            {
                if (behaviour.TryDealDamage() == false)
                {
                    behaviour.Move();
                }
                else
                {
                    ObjectPool<ParticleSystem> pool = EffectHolder.Instance.GetPool(EffectType.Merge);
                    ParticleSystem particle = pool.Get();
                    particle.transform.position = targetData.TargetHealth.transform.position;
                    pool.ReleaseParticle(particle).Forget();

                    Dispose();
                }

                await UniTask.Yield();
            }
        }
    }
}