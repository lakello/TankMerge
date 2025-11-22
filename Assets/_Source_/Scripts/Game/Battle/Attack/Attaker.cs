namespace MiniIT.GAME.Battle
{
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UtilsModule.Other;

    public class Attaker : MonoBehaviour
    {
        [SerializeField] private Tower tower;
        [SerializeField] private float reloadTime;

        [SerializeField]
        [SerializeReference]
        private IBulletBehaviour bulletBehaviour;

        private Fraction targetFraction;

        private CancellationTokenSource ctx = null;

        private void OnDisable()
        {
            ctx?.Cancel();
        }

        public void StartAttack(BattleData data)
        {
            targetFraction = data.TargetFraction;

            ctx = new CancellationTokenSource();

            AttackBehaviour(ctx.Token).Forget();
        }

        public void StopAttack()
        {
            ctx?.Cancel();
        }

        private async UniTaskVoid AttackBehaviour(CancellationToken token)
        {
            Health targetHealth = null;

            while (token.IsCancellationRequested == false)
            {
                if (targetHealth == null || targetHealth.IsAlive == false)
                {
                    targetHealth = GlobalData.Container.Resolve<BattleTargetHolder>().GetRandomTargetHealth(targetFraction);
                }

                if (targetHealth != null)
                {
                    if (tower.IsLook(targetHealth.transform.position) == false)
                    {
                        tower.Look(targetHealth.transform.position);

                        await UniTask.Yield();
                        continue;
                    }

                    _ = new Bullet(bulletBehaviour, targetHealth);
                }

                await UniTask.WaitForSeconds(reloadTime, cancellationToken: token);
            }
        }
    }
}