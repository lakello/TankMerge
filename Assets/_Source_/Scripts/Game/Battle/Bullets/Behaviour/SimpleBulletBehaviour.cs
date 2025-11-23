namespace MiniIT.GAME.BATTLE
{
    using System;
    using UnityEngine;

    [Serializable]
    public class SimpleBulletBehaviour : BulletBehaviour<SimpleBulletBehaviour>
    {
        [SerializeField]
        private Transform startPoint;
        [SerializeField]
        private float dealDamageDistance;
        [SerializeField]
        private float defaultDamage;
        [SerializeField]
        private float speed = 10f;

        private float      damage;
        private BulletView view;
        private Health     targetHealth;

        protected override void InternalInit(BulletBehaviourData data)
        {
            targetHealth = data.TargetHealth;
            damage = defaultDamage * data.DamageMultiplier;

            view = BulletViewPoolHolder.Value.Get();
            view.transform.position = startPoint.position;
            view.transform.rotation = startPoint.rotation;
        }

        public override void Move()
        {
            if (view == null || targetHealth == null)
            {
                return;
            }

            Vector3 direction = (targetHealth.transform.position - view.transform.position).normalized;
            view.transform.position += direction * speed * Time.deltaTime;
        }

        public override bool TryDealDamage()
        {
            if (view == null || targetHealth == null)
            {
                return false;
            }

            float distanceToTarget = Vector3.Distance(view.transform.position, targetHealth.transform.position);

            if (distanceToTarget <= dealDamageDistance)
            {
                targetHealth.TakeDamage(damage);

                BulletViewPoolHolder.Value.Release(view);
                view = null;

                return true;
            }

            return false;
        }
    }
}