namespace MiniIT.GAME.BATTLE
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
        private float    multiplier;

        private CancellationTokenSource ctx = null;

        private void OnDisable()
        {
            ctx?.Cancel();
        }

        public void Init(float damageMultiplier)
        {
            multiplier = damageMultiplier;
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
            BulletBehaviourData bulletBehaviourData = new BulletBehaviourData()
            {
                DamageMultiplier = multiplier,
            };

            while (token.IsCancellationRequested == false)
            {
                if (bulletBehaviourData.TargetHealth == null || bulletBehaviourData.TargetHealth.IsAlive == false)
                {
                    bulletBehaviourData.TargetHealth =
                        GlobalData.Container.Resolve<BattleTargetHolder>().GetRandomTargetHealth(targetFraction);
                }

                if (bulletBehaviourData.TargetHealth != null)
                {
                    if (tower.IsLook(bulletBehaviourData.TargetHealth.transform.position) == false)
                    {
                        tower.Look(bulletBehaviourData.TargetHealth.transform.position);

                        await UniTask.Yield();
                        continue;
                    }

                    _ = new Bullet(bulletBehaviour, bulletBehaviourData);
                }

                await UniTask.WaitForSeconds(reloadTime, cancellationToken: token);
            }
        }
    }
}