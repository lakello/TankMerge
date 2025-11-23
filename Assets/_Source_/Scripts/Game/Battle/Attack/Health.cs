namespace MiniIT.GAME.Battle
{
    using AudioModule;
    using AudioModule.Extensions;
    using R3;
    using UnityEngine;

    public class Health : MonoBehaviour
    {
        [SerializeField]
        private float maxHealth;

        public bool IsAlive => Current is { Value: > 0, } && IsActive.Value;
        public bool IsMax { get; private set; }
        public ReactiveProperty<float> Current { get; } = new ReactiveProperty<float>(1);
        public ReactiveProperty<bool> IsActive { get; } = new ReactiveProperty<bool>(false);
        public float Max { get; private set; }

        public void Init(float healthMultiplier)
        {
            Current.Value = maxHealth * healthMultiplier;
            Max = Current.Value;
            IsMax = true;
        }

        public void Activate()
        {
            IsActive.Value = true;
        }

        public void TakeDamage(float damage)
        {
            IsMax = false;
            Current.Value -= damage;
            
            AudioID.TakeDamage.PlayOneShot();
        }

        private void OnDisable()
        {
            IsActive.Value = false;
        }
    }
}