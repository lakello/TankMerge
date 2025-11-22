namespace MiniIT.GAME.Particles
{
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Pool;

    public static class ParticleExtensions
    {
        public static async UniTaskVoid ReleaseParticle(
            this ObjectPool<ParticleSystem> pool,
            ParticleSystem particle,
            float duration = 0,
            CancellationToken token = default)
        {
            if (duration <= 0)
            {
                duration = particle.main.duration;
            }

            await UniTask.WaitForSeconds(duration, cancellationToken: token);

            if (token.IsCancellationRequested)
            {
                return;
            }

            pool?.Release(particle);
        }
		
        public static async UniTaskVoid ReleaseAfterTime<T>(
            this ObjectPool<T> pool,
            T obj,
            float duration,
            CancellationToken token = default)
            where T : class
        {
            await UniTask.WaitForSeconds(duration, cancellationToken: token);

            if (token.IsCancellationRequested)
            {
                return;
            }

            pool?.Release(obj);
        }
    }
}