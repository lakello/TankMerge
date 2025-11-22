namespace MiniIT.GAME.Battle
{
    using R3;
    using UnityEngine;

    public class Health : MonoBehaviour
    {
        [SerializeField]
        private float maxHealth;

        private bool isInitialized;

        public bool IsAlive => Current is { Value: > 0, } && isInitialized;
        public bool IsMax { get; private set; }
        public ReactiveProperty<float> Current { get; } = new ReactiveProperty<float>(1);

        public void Init(float healthMultiplier)
        {
            Current.Value = maxHealth * healthMultiplier;
            IsMax = true;
            isInitialized = true;
        }

        public void TakeDamage(float damage)
        {
            IsMax = false;
            Current.Value -= damage;
        }

        private void OnDisable()
        {
            isInitialized = false;
        }
    }
}